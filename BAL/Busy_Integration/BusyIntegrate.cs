using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Busy_Integration
{
    public class BusyIntegrate
    {
        //Anurag
        //insert item from busy
        //updation on 14-06-2021

        public int InsertItems_Busy(string ItemName, string Unit, bool Active, decimal StdPack, decimal Mrp, decimal Dp, decimal Rp, string ParentName, string ItemCode, string Syncid, string ItemType, string DispName, string PriceGroup, string primaryunit, string Secondaryunit, decimal PrimaryUnitfactor, decimal SecondaryUnitfactor, decimal MOQ, bool Promoted, decimal cgstper, decimal sgstper, decimal igstper, string Segment = "0", string ProductClass = "0")
        {

            DbParameter[] dbParam = new DbParameter[25];
            dbParam[0] = new DbParameter("@UnderID", DbParameter.DbType.Int, 1, Convert.ToInt32(ParentName));
            dbParam[1] = new DbParameter("@ItemName", DbParameter.DbType.VarChar, 50, ItemName);
            dbParam[2] = new DbParameter("@ItemCode", DbParameter.DbType.VarChar, 50, ItemCode);
            dbParam[3] = new DbParameter("@Unit", DbParameter.DbType.VarChar, 10, Unit);
            dbParam[4] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[5] = new DbParameter("@StdPack", DbParameter.DbType.Decimal, 12, StdPack);
            dbParam[6] = new DbParameter("@Syncid", DbParameter.DbType.VarChar, 150, Syncid);
            dbParam[7] = new DbParameter("@Mrp", DbParameter.DbType.Decimal, 12, Mrp);
            dbParam[8] = new DbParameter("@Dp", DbParameter.DbType.Decimal, 12, Dp);
            dbParam[9] = new DbParameter("@Rp", DbParameter.DbType.Decimal, 12, Rp);
            dbParam[10] = new DbParameter("@ClassId", DbParameter.DbType.Int, 1, Convert.ToInt32(ProductClass));
            dbParam[11] = new DbParameter("@SegmentId", DbParameter.DbType.Int, 1, Convert.ToInt32(Segment));
            dbParam[12] = new DbParameter("@PriceGroup", DbParameter.DbType.VarChar, 20, PriceGroup);
            dbParam[13] = new DbParameter("@ItemType", DbParameter.DbType.VarChar, 20, ItemType);
            dbParam[14] = new DbParameter("@DispName", DbParameter.DbType.VarChar, 20, DispName);
            dbParam[15] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            dbParam[16] = new DbParameter("@primaryunit", DbParameter.DbType.VarChar, 20, primaryunit);
            dbParam[17] = new DbParameter("@Secondaryunit", DbParameter.DbType.VarChar, 20, Secondaryunit);
            dbParam[18] = new DbParameter("@PrimaryUnitfactor", DbParameter.DbType.Decimal, 12, PrimaryUnitfactor);

            dbParam[19] = new DbParameter("@SecondaryUnitfactor", DbParameter.DbType.Decimal, 12, SecondaryUnitfactor);

            dbParam[20] = new DbParameter("@MOQ", DbParameter.DbType.Decimal, 12, MOQ);
            dbParam[21] = new DbParameter("@Promoted", DbParameter.DbType.Bit, 1, Promoted);
            dbParam[22] = new DbParameter("@cgstper", DbParameter.DbType.Decimal, 12, cgstper);
            dbParam[23] = new DbParameter("@sgstper", DbParameter.DbType.Decimal, 12, sgstper);
            dbParam[24] = new DbParameter("@igstper", DbParameter.DbType.Decimal, 12, igstper);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertItemForm_Busy", dbParam);
            return Convert.ToInt32(dbParam[15].Value);
            //DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertItemForm_Busy", dbParam, sqlconn);
            //return Convert.ToInt32(dbParam[15].Value);
        }

        public int UpdateItems_Busy(Int32 ItemId, string ItemName, string Unit, bool Active, decimal StdPack, decimal Mrp, decimal Dp, decimal Rp, string ParentName, string ItemCode, string Syncid, string ItemType, string DispName, string PriceGroup, string primaryunit, string Secondaryunit, decimal PrimaryUnitfactor, decimal SecondaryUnitfactor, decimal MOQ, bool Promoted, decimal cgstper, decimal sgstper, decimal igstper, string Segment = "0", string ProductClass = "0")
        {
            DbParameter[] dbParam = new DbParameter[26];
            dbParam[0] = new DbParameter("@ItemId", DbParameter.DbType.Int, 4, ItemId);
            dbParam[1] = new DbParameter("@UnderID", DbParameter.DbType.Int, 1, Convert.ToInt32(ParentName));
            dbParam[2] = new DbParameter("@ItemName", DbParameter.DbType.VarChar, 50, ItemName);
            dbParam[3] = new DbParameter("@ItemCode", DbParameter.DbType.VarChar, 50, ItemCode);
            dbParam[4] = new DbParameter("@Unit", DbParameter.DbType.VarChar, 10, Unit);
            dbParam[5] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[6] = new DbParameter("@StdPack", DbParameter.DbType.Decimal, 12, StdPack);
            dbParam[7] = new DbParameter("@Syncid", DbParameter.DbType.VarChar, 150, Syncid);
            dbParam[8] = new DbParameter("@Mrp", DbParameter.DbType.Decimal, 12, Mrp);
            dbParam[9] = new DbParameter("@Dp", DbParameter.DbType.Decimal, 12, Dp);
            dbParam[10] = new DbParameter("@Rp", DbParameter.DbType.Decimal, 12, Rp);
            dbParam[11] = new DbParameter("@ClassId", DbParameter.DbType.Int, 1, Convert.ToInt32(ProductClass));
            dbParam[12] = new DbParameter("@SegmentId", DbParameter.DbType.Int, 1, Convert.ToInt32(Segment));
            dbParam[13] = new DbParameter("@PriceGroup", DbParameter.DbType.VarChar, 20, PriceGroup);
            dbParam[14] = new DbParameter("@ItemType", DbParameter.DbType.VarChar, 20, ItemType);
            dbParam[15] = new DbParameter("@DispName", DbParameter.DbType.VarChar, 20, DispName);
            dbParam[16] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            dbParam[17] = new DbParameter("@primaryunit", DbParameter.DbType.VarChar, 20, primaryunit);
            dbParam[18] = new DbParameter("@Secondaryunit", DbParameter.DbType.VarChar, 20, Secondaryunit);
            dbParam[19] = new DbParameter("@PrimaryUnitfactor", DbParameter.DbType.Decimal, 12, PrimaryUnitfactor);

            dbParam[20] = new DbParameter("@SecondaryUnitfactor", DbParameter.DbType.Decimal, 12, SecondaryUnitfactor);

            dbParam[21] = new DbParameter("@MOQ", DbParameter.DbType.Decimal, 12, MOQ);
            dbParam[22] = new DbParameter("@Promoted", DbParameter.DbType.Bit, 1, Promoted);
            dbParam[23] = new DbParameter("@cgstper", DbParameter.DbType.Decimal, 12, cgstper);
            dbParam[24] = new DbParameter("@sgstper", DbParameter.DbType.Decimal, 12, sgstper);
            dbParam[25] = new DbParameter("@igstper", DbParameter.DbType.Decimal, 12, igstper);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdateItemForm_Busy", dbParam);
            return Convert.ToInt32(dbParam[16].Value);
            //DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdateItemForm_Busy", dbParam, sqlconn);
            //return Convert.ToInt32(dbParam[16].Value);
        }

        public int Insert(string UserName, string Password, string email, int Roleid, bool isAdmin, string DeptId, string DesgId, string empName, decimal CostCentre, string empSyncId)
        {
            DbParameter[] dbParam = new DbParameter[13];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.BigInt, 1, 0);
            dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 100, UserName);
            dbParam[2] = new DbParameter("@Pwd", DbParameter.DbType.VarChar, 20, Password);
            dbParam[3] = new DbParameter("@Active ", DbParameter.DbType.Bit, 1, isAdmin);
            dbParam[4] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, email);
            dbParam[5] = new DbParameter("@RoleId", DbParameter.DbType.Int, 4, Roleid);
            dbParam[6] = new DbParameter("@DeptId", DbParameter.DbType.Int, 4, DeptId);
            dbParam[7] = new DbParameter("@DesigId", DbParameter.DbType.Int, 4, DesgId);
            dbParam[8] = new DbParameter("@EmpName", DbParameter.DbType.VarChar, 50, empName);
            dbParam[9] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[10] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[11] = new DbParameter("@CostCentre", DbParameter.DbType.Decimal, 15, CostCentre);
            dbParam[12] = new DbParameter("@EmpSyncId", DbParameter.DbType.VarChar, 30, empSyncId);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastLogin__Busy", dbParam);
            return Convert.ToInt32(dbParam[10].Value);
        }


        public int InsertSalespersons(string SMName, string Pin, string SalesPerType, string DeviceNo, string DSRAllowDays, string isAdmin, string Address1, string Address2,
         string CityId, string Email, string Mobile, string Remarks, string RoleId, Int32 UserId, string SyncId, string UnderId, int GradeId, int DeptId, int DesgId, string DOB, string DOA, string ResCenID, string BlockReason, int BlockBy, string EmpName, bool AllowChangeCity, int MeetAllowDays, bool MobileAccess, string FromTime, string ToTime, string Interval, string Uploadinterval, string Accuracy, string Sendpushntification, string BatteryRecord,
string groupcode, string retryinterval, bool gpsloc, bool mobileloc, string sys_flag, string Alarm, int alarmduration, string sendsms, string sendsmsperson, string lat, string longi)
        {
            DbParameter[] dbParam = new DbParameter[48];
            dbParam[0] = new DbParameter("@SMName", DbParameter.DbType.VarChar, 50, SMName);
            dbParam[1] = new DbParameter("@SalesPerType", DbParameter.DbType.VarChar, 50, SalesPerType);
            dbParam[2] = new DbParameter("@UnderId", DbParameter.DbType.Int, 10, UnderId);
            dbParam[3] = new DbParameter("@DeptId", DbParameter.DbType.Int, 10, DeptId);
            dbParam[4] = new DbParameter("@DesgId", DbParameter.DbType.Int, 10, DesgId);
            dbParam[5] = new DbParameter("@GradeId", DbParameter.DbType.Int, 10, DBNull.Value);
            dbParam[6] = new DbParameter("@ResCenID", DbParameter.DbType.Int, 10, 1);
            if (!string.IsNullOrEmpty(DOB))
                dbParam[7] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DOB);
            else
                dbParam[7] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
            if (!string.IsNullOrEmpty(DOA))
                dbParam[8] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
            else
                dbParam[8] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
            dbParam[9] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
            dbParam[10] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
            dbParam[11] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
            dbParam[12] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
            dbParam[13] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 15, Mobile);
            dbParam[14] = new DbParameter("@DeviceNo", DbParameter.DbType.VarChar, 50, DeviceNo);
            dbParam[15] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
            dbParam[16] = new DbParameter("@Remarks", DbParameter.DbType.Text, 1000, Remarks);
            dbParam[17] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[18] = new DbParameter("@DSRAllowDays", DbParameter.DbType.Int, 10, DSRAllowDays);
            dbParam[19] = new DbParameter("@isAdmin", DbParameter.DbType.VarChar, 1, isAdmin);
            dbParam[20] = new DbParameter("@UserId", DbParameter.DbType.Int, 10, UserId);
            dbParam[21] = new DbParameter("@RoleId", DbParameter.DbType.Int, 10, RoleId);
            if (isAdmin == "1")
            {
                dbParam[22] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 15, DBNull.Value);
                dbParam[23] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
                dbParam[24] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
            }
            else
            {
                dbParam[22] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 15, DateTime.Now.ToUniversalTime().AddSeconds(19800));
                dbParam[23] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
                dbParam[24] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, BlockBy);
            }
            dbParam[25] = new DbParameter("@EmpName", DbParameter.DbType.VarChar, 50, EmpName);
            dbParam[26] = new DbParameter("@AllowChangeCity", DbParameter.DbType.Bit, 1, AllowChangeCity);
            dbParam[27] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[28] = new DbParameter("@MeetAllowDays", DbParameter.DbType.Int, 3, MeetAllowDays);
            dbParam[29] = new DbParameter("@MobileAccess", DbParameter.DbType.Bit, 1, MobileAccess);

            dbParam[30] = new DbParameter("@FromTime", DbParameter.DbType.VarChar, 50, FromTime);
            dbParam[31] = new DbParameter("@ToTime", DbParameter.DbType.VarChar, 50, ToTime);
            dbParam[32] = new DbParameter("@Interval", DbParameter.DbType.VarChar, 10, Interval);
            dbParam[33] = new DbParameter("@Uploadinterval", DbParameter.DbType.VarChar, 10, Uploadinterval);
            dbParam[34] = new DbParameter("@Accuracy", DbParameter.DbType.VarChar, 10, Accuracy);
            dbParam[35] = new DbParameter("@Sendpushntification", DbParameter.DbType.VarChar, 1, Sendpushntification);
            dbParam[36] = new DbParameter("@BatteryRecord", DbParameter.DbType.VarChar, 1, BatteryRecord);
            dbParam[37] = new DbParameter("@groupcode", DbParameter.DbType.VarChar, 10, groupcode);
            dbParam[38] = new DbParameter("@retryinterval", DbParameter.DbType.VarChar, 10, retryinterval);
            dbParam[39] = new DbParameter("@gpsloc", DbParameter.DbType.Bit, 1, gpsloc);
            dbParam[40] = new DbParameter("@mobileloc", DbParameter.DbType.Bit, 1, mobileloc);
            dbParam[41] = new DbParameter("@sys_flag", DbParameter.DbType.VarChar, 10, sys_flag);
            dbParam[42] = new DbParameter("@Alarm", DbParameter.DbType.VarChar, 1, Alarm);
            dbParam[43] = new DbParameter("@alarmduration", DbParameter.DbType.Int, 3, alarmduration);
            dbParam[44] = new DbParameter("@sendsms", DbParameter.DbType.VarChar, 1, sendsms);
            dbParam[45] = new DbParameter("@sendsmsperson", DbParameter.DbType.VarChar, 1, sendsmsperson);
            dbParam[46] = new DbParameter("@lat", DbParameter.DbType.VarChar, 1, lat);
            dbParam[47] = new DbParameter("@longi", DbParameter.DbType.VarChar, 10, longi);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertSalesRepForm_Busy", dbParam);
            return Convert.ToInt32(dbParam[27].Value);
        }

        public int InsertDistributorBusy(string Compcode, string PartyName, string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, bool Active, string Phone, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal CreditLimit, decimal OutStanding, Int32 SMID, string DOA, string DOB, int Areaid, string AreaName, string BeatName, string TCountryName, string TStateName, string CityName, int countryid, int stateid, int regionid, int distictid, int citytypeid, int cityconveyancetype, int Beatid, int Roleid, string UserName, string DistType, int createduserid, string UnderId, int PartyType, string Partygstin = "")
        {
            DbParameter[] dbParam = new DbParameter[45];
            dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, PartyName);
            dbParam[1] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
            dbParam[2] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
            dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
            dbParam[4] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
            dbParam[5] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
            dbParam[6] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
            dbParam[7] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
            dbParam[8] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
            dbParam[9] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            dbParam[10] = new DbParameter("@DistributorName", DbParameter.DbType.VarChar, 150, DistName);
            dbParam[11] = new DbParameter("@CreatedUserId", DbParameter.DbType.Int, 10, createduserid);
            dbParam[12] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            if (!Active)
            {

                dbParam[13] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
            }
            else
            {
                dbParam[13] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
            }
            dbParam[14] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
            dbParam[15] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
            dbParam[16] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
            dbParam[17] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
            dbParam[18] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
            dbParam[19] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
            dbParam[20] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);
            dbParam[21] = new DbParameter("@BeatId", DbParameter.DbType.Int, 10, Beatid);
            dbParam[22] = new DbParameter("@SMID", DbParameter.DbType.Int, 10, SMID);
            dbParam[23] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            if (DOA != "")
            {
                dbParam[24] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
            }
            else
            {
                dbParam[24] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }

            if (DOB != "")
            {
                dbParam[25] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
            }
            else
            {
                dbParam[25] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }
            dbParam[26] = new DbParameter("@AreaId", DbParameter.DbType.Int, 10, Areaid);

            if (Partygstin == "")
            {
                dbParam[27] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, DBNull.Value);
            }
            else
            {
                dbParam[27] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);
            }

            dbParam[28] = new DbParameter("@UnderId", DbParameter.DbType.VarChar, 50, UnderId);
            dbParam[29] = new DbParameter("@TCountryName", DbParameter.DbType.VarChar, 50, TCountryName);
            dbParam[30] = new DbParameter("@TStateName", DbParameter.DbType.VarChar, 50, TStateName);
            dbParam[31] = new DbParameter("@countryid", DbParameter.DbType.Int, 10, countryid);
            dbParam[32] = new DbParameter("@regionid", DbParameter.DbType.Int, 10, regionid);
            dbParam[33] = new DbParameter("@stateid", DbParameter.DbType.Int, 10, stateid);
            dbParam[34] = new DbParameter("@distictid", DbParameter.DbType.Int, 10, distictid);
            dbParam[35] = new DbParameter("@citytypeid", DbParameter.DbType.Int, 10, citytypeid);
            dbParam[36] = new DbParameter("@cityconveyancetype", DbParameter.DbType.Int, 10, cityconveyancetype);
            dbParam[37] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 10, Compcode);
            dbParam[38] = new DbParameter("@PartyType", DbParameter.DbType.Int, 12, DBNull.Value);
            dbParam[39] = new DbParameter("@DistType", DbParameter.DbType.VarChar, 20, DistType);
            dbParam[40] = new DbParameter("@Roleid", DbParameter.DbType.Int, 12, Roleid);
            dbParam[41] = new DbParameter("@UserName", DbParameter.DbType.VarChar, 100, UserName);
            dbParam[42] = new DbParameter("@CityName", DbParameter.DbType.VarChar, 100, CityName);
            dbParam[43] = new DbParameter("@AreaName", DbParameter.DbType.VarChar, 100, AreaName);
            dbParam[44] = new DbParameter("@BeatName", DbParameter.DbType.VarChar, 100, BeatName);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertIDistributorForm_Busy", dbParam);
            return Convert.ToInt32(dbParam[23].Value);
        }

        public int UpdateDistributorBusy(int DistID, string Compcode, string PartyName, string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, bool Active, string Phone, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal CreditLimit, decimal OutStanding, Int32 SMID, string DOA, string DOB, int Areaid, string AreaName, string BeatName, string TCountryName, string TStateName, string CityName, int countryid, int stateid, int regionid, int distictid, int citytypeid, int cityconveyancetype, int Beatid, int Roleid, string UserName, string DistType, int createduserid, string UnderId, int PartyType, string Partygstin = "")
        {
            DbParameter[] dbParam = new DbParameter[46];
            dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, PartyName);
            dbParam[1] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
            dbParam[2] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
            dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
            dbParam[4] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
            dbParam[5] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
            dbParam[6] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
            dbParam[7] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
            dbParam[8] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
            dbParam[9] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            dbParam[10] = new DbParameter("@DistributorName", DbParameter.DbType.VarChar, 150, DistName);
            dbParam[11] = new DbParameter("@CreatedUserId", DbParameter.DbType.Int, 10, createduserid);
            dbParam[12] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            if (!Active)
            {
                dbParam[13] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
            }
            else
            {
                dbParam[13] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
            }
            dbParam[14] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
            dbParam[15] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
            dbParam[16] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
            dbParam[17] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
            dbParam[18] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
            dbParam[19] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
            dbParam[20] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);
            dbParam[21] = new DbParameter("@BeatId", DbParameter.DbType.Int, 10, Beatid);
            dbParam[22] = new DbParameter("@SMID", DbParameter.DbType.Int, 10, SMID);
            dbParam[23] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            if (DOA != "")
            {
                dbParam[24] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
            }
            else
            {
                dbParam[24] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }

            if (DOB != "")
            {
                dbParam[25] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
            }
            else
            {
                dbParam[25] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
            }
            dbParam[26] = new DbParameter("@AreaId", DbParameter.DbType.Int, 10, Areaid);

            if (Partygstin == "")
            {
                dbParam[27] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, DBNull.Value);
            }
            else
            {
                dbParam[27] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);
            }

            dbParam[28] = new DbParameter("@UnderId", DbParameter.DbType.VarChar, 50, UnderId);
            dbParam[29] = new DbParameter("@TCountryName", DbParameter.DbType.VarChar, 50, TCountryName);
            dbParam[30] = new DbParameter("@TStateName", DbParameter.DbType.VarChar, 50, TStateName);
            dbParam[31] = new DbParameter("@countryid", DbParameter.DbType.Int, 10, countryid);
            dbParam[32] = new DbParameter("@regionid", DbParameter.DbType.Int, 10, regionid);
            dbParam[33] = new DbParameter("@stateid", DbParameter.DbType.Int, 10, stateid);
            dbParam[34] = new DbParameter("@distictid", DbParameter.DbType.Int, 10, distictid);
            dbParam[35] = new DbParameter("@citytypeid", DbParameter.DbType.Int, 10, citytypeid);
            dbParam[36] = new DbParameter("@cityconveyancetype", DbParameter.DbType.Int, 10, cityconveyancetype);
            dbParam[37] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 10, Compcode);
            dbParam[38] = new DbParameter("@PartyType", DbParameter.DbType.Int, 12, PartyType);
            dbParam[39] = new DbParameter("@DistID", DbParameter.DbType.Int, 12, DistID);
            dbParam[40] = new DbParameter("@DistType", DbParameter.DbType.VarChar, 20, DistType);
            dbParam[41] = new DbParameter("@Roleid", DbParameter.DbType.Int, 12, Roleid);
            dbParam[42] = new DbParameter("@UserName", DbParameter.DbType.VarChar, 100, UserName);
            dbParam[43] = new DbParameter("@CityName", DbParameter.DbType.VarChar, 100, CityName);
            dbParam[44] = new DbParameter("@AreaName", DbParameter.DbType.VarChar, 100, AreaName);
            dbParam[45] = new DbParameter("@BeatName", DbParameter.DbType.VarChar, 100, BeatName);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdateDistributorForm_Busy", dbParam);
            return Convert.ToInt32(dbParam[23].Value);
        }

        public int insertsaleinvheader_Busy(string Docid, string VDate, decimal taxamt, decimal Billamount, decimal Roundoff, decimal discamo, string SyncId, string compcode = "")
        {
            DbParameter[] dbParam = new DbParameter[9];
            dbParam[0] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, Docid);
            dbParam[1] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(VDate));
            dbParam[2] = new DbParameter("@taxamt", DbParameter.DbType.Decimal, 20, taxamt);
            dbParam[3] = new DbParameter("@Billamount", DbParameter.DbType.Decimal, 20, Billamount);
            dbParam[4] = new DbParameter("@Roundoff", DbParameter.DbType.Decimal, 20, Roundoff);
            dbParam[5] = new DbParameter("@discamo", DbParameter.DbType.Decimal, 20, discamo);
            dbParam[6] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            dbParam[7] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
            dbParam[8] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_Insertsaleinvoiceheader_Busy", dbParam);
            return Convert.ToInt32(dbParam[8].Value);
        }

        public int insertsaleinvdetail_Busy(int DistInvId, int sno, string Docid, string VDate, decimal taxamt, decimal Qty, decimal Rate, decimal Amount, string SyncId, string itemname, string ItemMasterid, string Unit, string compcode = "")
        {
            DbParameter[] dbParam = new DbParameter[14];

            dbParam[0] = new DbParameter("@DistInvId", DbParameter.DbType.Int, 10, DistInvId);
            dbParam[1] = new DbParameter("@sno", DbParameter.DbType.Int, 10, sno);
            dbParam[2] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, Docid);
            dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(VDate));
            dbParam[4] = new DbParameter("@taxamt", DbParameter.DbType.Decimal, 20, taxamt);
            dbParam[5] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 20, Qty);
            dbParam[6] = new DbParameter("@Rate", DbParameter.DbType.Decimal, 20, Rate);
            dbParam[7] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 20, Amount);
            dbParam[8] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
            dbParam[9] = new DbParameter("@itemname", DbParameter.DbType.VarChar, 150, itemname);
            dbParam[10] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[12] = new DbParameter("@ItemId", DbParameter.DbType.VarChar, 10, ItemMasterid);
            dbParam[13] = new DbParameter("@Unit", DbParameter.DbType.VarChar, 10, Unit);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_Insertsaleinvoicedetail_Busy", dbParam);
            return Convert.ToInt32(dbParam[11].Value);
        }

        public int insertRetailersaleinvexpensedetail(string Docid, string Desc, decimal Amt, string compcode = "")
        {

            DbParameter[] dbParam = new DbParameter[5];
            dbParam[0] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, Docid);
            dbParam[1] = new DbParameter("@description", DbParameter.DbType.VarChar, 30, Desc);
            dbParam[2] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 20, Amt);
            dbParam[3] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
            dbParam[4] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_insertDistributorsaleinvexpensedetail_BUSY", dbParam);
            return Convert.ToInt32(dbParam[4].Value);
        }

        public int insertdistledger_Busy(string Syncid, string VDate, decimal amtdr, decimal amtcr, decimal amt, string narr, string VID, string VTYPE, string compcode = "")
        {
            DbParameter[] dbParam = new DbParameter[10];
            dbParam[0] = new DbParameter("@Syncid", DbParameter.DbType.VarChar, 30, Syncid);

            dbParam[1] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(VDate));
            dbParam[2] = new DbParameter("@amtdr", DbParameter.DbType.Decimal, 20, amtdr);
            dbParam[3] = new DbParameter("@amtcr", DbParameter.DbType.Decimal, 20, amtcr);
            dbParam[4] = new DbParameter("@amt", DbParameter.DbType.Decimal, 20, amt);
            dbParam[5] = new DbParameter("@Narration", DbParameter.DbType.VarChar, 300, narr);
            dbParam[6] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
            dbParam[7] = new DbParameter("@VTYPE", DbParameter.DbType.VarChar, 30, VTYPE);
            dbParam[8] = new DbParameter("@VID", DbParameter.DbType.VarChar, 30, VID);
            dbParam[9] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertDistributorLedgerForm_Busy", dbParam);
            return Convert.ToInt32(dbParam[9].Value);
        }
    }
}
