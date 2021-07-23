using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;


namespace BAL
{
   public class PartyBAL
   {
       public int InsertParty(string PartyName, string Address1, string Address2, Int32 CityId, Int32 AreaId, Int32 BeatId, Int32 UnderId, string Pin, string Mobile, string Phone, string Remark, string SyncId, string IndId, decimal Potential, bool Active, string BlockReason,
           int PartyType, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, int UserId, string DOA, string DOB, string Email, string Imgurl,string GSTINNo)
       {
           DbParameter[] dbParam = new DbParameter[31];
           dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, PartyName);
           dbParam[1] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
           dbParam[2] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
           dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
           dbParam[4] = new DbParameter("@AreaId", DbParameter.DbType.Int, 10, AreaId);
           dbParam[5] = new DbParameter("@BeatId", DbParameter.DbType.Int, 10, BeatId);
           dbParam[6] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
           dbParam[7] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
           dbParam[8] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
           dbParam[9] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
           dbParam[10] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
           dbParam[11] = new DbParameter("@IndId", DbParameter.DbType.Int, 10, IndId);
           dbParam[12] = new DbParameter("@Potential", DbParameter.DbType.Decimal, 10, Potential);
           dbParam[13] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
           dbParam[14] = new DbParameter("@UnderID", DbParameter.DbType.Int, 10, UnderId);
           dbParam[15] = new DbParameter("@PartyType", DbParameter.DbType.Int, 10, PartyType);
           dbParam[16] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
           dbParam[17] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
           dbParam[18] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
           dbParam[19] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
           dbParam[20] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
           if (!Active)
           {
               
               dbParam[21] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
               dbParam[22] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
               dbParam[23] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
           }
           else
           {
               dbParam[21] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
               dbParam[22] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
               dbParam[23] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
           }
           dbParam[24] = new DbParameter("@CreatedUserId", DbParameter.DbType.Int, 10, UserId);
           dbParam[25] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           if (DOA != "")
           {
               dbParam[26] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
           }
           else
           {
               dbParam[26] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }

           if (DOB != "")
           {
               dbParam[27] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
           }
           else
           {
               dbParam[27] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }
           dbParam[28] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
           dbParam[29] = new DbParameter("@Imgurl", DbParameter.DbType.VarChar, 300, Imgurl);
           dbParam[30] = new DbParameter("@GSTINNo", DbParameter.DbType.VarChar, 50, GSTINNo);
           //"Sp_InsertPartyForm"
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertPartyAstralForm", dbParam);
           return Convert.ToInt32(dbParam[25].Value);
       }

       public int UpdateParty(Int32 PartyId, string PartyName, string Address1, string Address2, Int32 CityId, Int32 AreaId, Int32 BeatId, Int32 UnderId, string Pin, string Mobile, string Phone, string Remark, string SyncId, string IndId, decimal Potential, bool Active, string BlockReason,
          int PartyType, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, int UserId, string DOA, string DOB, string Email, string Imgurl,string GSTINNo)
       {
           DbParameter[] dbParam = new DbParameter[32];
           dbParam[0] = new DbParameter("@PartyId", DbParameter.DbType.Int, 10, PartyId);
           dbParam[1] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, PartyName);
           dbParam[2] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
           dbParam[3] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
           dbParam[4] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
           dbParam[5] = new DbParameter("@AreaId", DbParameter.DbType.Int, 10, AreaId);
           dbParam[6] = new DbParameter("@BeatId", DbParameter.DbType.Int, 10, BeatId);
           dbParam[7] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
           dbParam[8] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
           dbParam[9] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
           dbParam[10] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
           dbParam[11] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
           dbParam[12] = new DbParameter("@IndId", DbParameter.DbType.Int, 10, IndId);
           dbParam[13] = new DbParameter("@Potential", DbParameter.DbType.Decimal, 10, Potential);
           dbParam[14] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
           dbParam[15] = new DbParameter("@UnderID", DbParameter.DbType.Int, 10, UnderId);
           dbParam[16] = new DbParameter("@PartyType", DbParameter.DbType.Int, 10, PartyType);
           dbParam[17] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
           dbParam[18] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
           dbParam[19] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
           dbParam[20] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
           dbParam[21] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
           if (!Active)
           {
               dbParam[22] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
               dbParam[23] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
               dbParam[24] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
           }
           else
           {
               dbParam[22] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
               dbParam[23] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
               dbParam[24] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
           }
           dbParam[25] = new DbParameter("@CreatedUserId", DbParameter.DbType.Int, 10, UserId);
           dbParam[26] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           if (DOA != "")
           {
               dbParam[27] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
           }
           else
           {
               dbParam[27] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }

           if (DOB != "")
           {
               dbParam[28] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
           }
           else
           {
               dbParam[28] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }
           dbParam[29] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
           dbParam[30] = new DbParameter("@Imgurl", DbParameter.DbType.VarChar, 300, Imgurl);
           dbParam[31] = new DbParameter("@GSTINNo", DbParameter.DbType.VarChar, 50, GSTINNo);
           //Sp_UpdatePartyForm 
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdatePartyAstralForm", dbParam);
           return Convert.ToInt32(dbParam[26].Value);
       }

       public int InsertPartyFromMobile(string PartyName, string Address1, string Address2, Int32 CityId, Int32 AreaId, Int32 BeatId, Int32 UnderId, string Pin, string Mobile, string Phone, string Remark, string SyncId, string IndId, decimal Potential, bool Active, string BlockReason,
           int PartyType, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, int UserId, string DOA, string DOB, string Email, string Imgurl, string GSTINNo, string Android_Id)
       {
           DbParameter[] dbParam = new DbParameter[32];
           dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, PartyName);
           dbParam[1] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
           dbParam[2] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
           dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
           dbParam[4] = new DbParameter("@AreaId", DbParameter.DbType.Int, 10, AreaId);
           dbParam[5] = new DbParameter("@BeatId", DbParameter.DbType.Int, 10, BeatId);
           dbParam[6] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
           dbParam[7] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
           dbParam[8] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
           dbParam[9] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
           dbParam[10] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
           dbParam[11] = new DbParameter("@IndId", DbParameter.DbType.Int, 10, IndId);
           dbParam[12] = new DbParameter("@Potential", DbParameter.DbType.Decimal, 10, Potential);
           dbParam[13] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
           dbParam[14] = new DbParameter("@UnderID", DbParameter.DbType.Int, 10, UnderId);
           dbParam[15] = new DbParameter("@PartyType", DbParameter.DbType.Int, 10, PartyType);
           dbParam[16] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
           dbParam[17] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
           dbParam[18] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
           dbParam[19] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
           dbParam[20] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
           if (!Active)
           {

               dbParam[21] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
               dbParam[22] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
               dbParam[23] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
           }
           else
           {
               dbParam[21] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
               dbParam[22] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
               dbParam[23] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
           }
           dbParam[24] = new DbParameter("@CreatedUserId", DbParameter.DbType.Int, 10, UserId);
           dbParam[25] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           if (DOA != "")
           {
               dbParam[26] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
           }
           else
           {
               dbParam[26] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }

           if (DOB != "")
           {
               dbParam[27] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
           }
           else
           {
               dbParam[27] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }
           dbParam[28] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
           dbParam[29] = new DbParameter("@Imgurl", DbParameter.DbType.VarChar, 300, Imgurl);
           dbParam[30] = new DbParameter("@GSTINNo", DbParameter.DbType.VarChar, 50, GSTINNo);
           dbParam[31] = new DbParameter("@Android_Id", DbParameter.DbType.VarChar, 30, Android_Id);
           //"Sp_InsertPartyForm"
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertPartyFromMobile", dbParam);
           return Convert.ToInt32(dbParam[25].Value);
       }

       public int UpdatePartyFromMobile(Int32 PartyId, string PartyName, string Address1, string Address2, Int32 CityId, Int32 AreaId, Int32 BeatId, Int32 UnderId, string Pin, string Mobile, string Phone, string Remark, string SyncId, string IndId, decimal Potential, bool Active, string BlockReason,
        int PartyType, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, int UserId, string DOA, string DOB, string Email, string Imgurl, string GSTINNo, string Android_Id)
       {
           DbParameter[] dbParam = new DbParameter[33];
           dbParam[0] = new DbParameter("@PartyId", DbParameter.DbType.Int, 10, PartyId);
           dbParam[1] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, PartyName);
           dbParam[2] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
           dbParam[3] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
           dbParam[4] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
           dbParam[5] = new DbParameter("@AreaId", DbParameter.DbType.Int, 10, AreaId);
           dbParam[6] = new DbParameter("@BeatId", DbParameter.DbType.Int, 10, BeatId);
           dbParam[7] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
           dbParam[8] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
           dbParam[9] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
           dbParam[10] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
           dbParam[11] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
           dbParam[12] = new DbParameter("@IndId", DbParameter.DbType.Int, 10, IndId);
           dbParam[13] = new DbParameter("@Potential", DbParameter.DbType.Decimal, 10, Potential);
           dbParam[14] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
           dbParam[15] = new DbParameter("@UnderID", DbParameter.DbType.Int, 10, UnderId);
           dbParam[16] = new DbParameter("@PartyType", DbParameter.DbType.Int, 10, PartyType);
           dbParam[17] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
           dbParam[18] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
           dbParam[19] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
           dbParam[20] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
           dbParam[21] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
           if (!Active)
           {
               dbParam[22] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
               dbParam[23] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
               dbParam[24] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
           }
           else
           {
               dbParam[22] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
               dbParam[23] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
               dbParam[24] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
           }
           dbParam[25] = new DbParameter("@CreatedUserId", DbParameter.DbType.Int, 10, UserId);
           dbParam[26] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           if (DOA != "")
           {
               dbParam[27] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
           }
           else
           {
               dbParam[27] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }

           if (DOB != "")
           {
               dbParam[28] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
           }
           else
           {
               dbParam[28] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }
           dbParam[29] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
           dbParam[30] = new DbParameter("@Imgurl", DbParameter.DbType.VarChar, 300, Imgurl);
           dbParam[31] = new DbParameter("@GSTINNo", DbParameter.DbType.VarChar, 50, GSTINNo);
           dbParam[32] = new DbParameter("@Android_Id", DbParameter.DbType.VarChar, 50, Android_Id);
        
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdatePartyFromMobile", dbParam);
           return Convert.ToInt32(dbParam[26].Value);
       }


       public int delete(string PartyId)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@PartyId", DbParameter.DbType.Int, 1, Convert.ToInt32(PartyId));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteParty", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
       public int InsertPItemStock(int STKId, string STKDocId, int UserId, string VDate, int VisId, int SMId, int PartyId, string PartyCode, int AreaId, int ItemId, decimal Qty, string Android_Id, string Created_date, string TimeStamp)
       {

           DbParameter[] dbParam = new DbParameter[16];
           dbParam[0] = new DbParameter("@PartySTKId", DbParameter.DbType.Int, 4, STKId);
           dbParam[1] = new DbParameter("@STKDocId", DbParameter.DbType.VarChar, 30, STKDocId);
           dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
           dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
           dbParam[4] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
           dbParam[5] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
           dbParam[6] = new DbParameter("@PartyCode", DbParameter.DbType.VarChar, 150, PartyCode);
           dbParam[7] = new DbParameter("@AreaId", DbParameter.DbType.Int, 4, AreaId);
           dbParam[8] = new DbParameter("@ItemId", DbParameter.DbType.Int, 4, ItemId);
           dbParam[9] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 20, Qty);
           dbParam[10] = new DbParameter("@Android_Id", DbParameter.DbType.VarChar, 150, Android_Id);
           dbParam[11] = new DbParameter("@Created_date", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(Created_date));
           dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
           dbParam[13] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
           dbParam[14] = new DbParameter("@TimeStamp", DbParameter.DbType.VarChar, 4, TimeStamp);
           dbParam[15] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransPartyStock_Ins", dbParam);
           return Convert.ToInt32(dbParam[15].Value);
       }
       public int InsertPItemStock(string STKDocId, int UserId, string VDate, int VisId, int SMId, int PartyId, string PartyCode, int AreaId, int ItemId, decimal Qty, string Android_Id, string Created_date, string TimeStamp)
       {

           DbParameter[] dbParam = new DbParameter[16];
           dbParam[0] = new DbParameter("@PartySTKId", DbParameter.DbType.Int, 4, 0);
           dbParam[1] = new DbParameter("@STKDocId", DbParameter.DbType.VarChar, 30, STKDocId);
           dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
           dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
           dbParam[4] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
           dbParam[5] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, Convert.ToInt32(PartyId));
           dbParam[6] = new DbParameter("@PartyCode", DbParameter.DbType.VarChar, 150, PartyCode);
           dbParam[7] = new DbParameter("@AreaId", DbParameter.DbType.Int, 4, AreaId);
           dbParam[8] = new DbParameter("@ItemId", DbParameter.DbType.Int, 4, ItemId);
           dbParam[9] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 20, Qty);
           dbParam[10] = new DbParameter("@Android_Id", DbParameter.DbType.VarChar, 150, Android_Id);
           dbParam[11] = new DbParameter("@Created_date", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(Created_date));
           dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
           dbParam[13] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
           dbParam[14] = new DbParameter("@TimeStamp", DbParameter.DbType.VarChar, 20, TimeStamp);
           dbParam[15] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransPartyStock_Ins", dbParam);
           return Convert.ToInt32(dbParam[15].Value);
       }
       public int InsertParty_Tally(string PartyName, string Distributor, string Address1, string Address2, Int32 CityId, Int32 AreaId, Int32 BeatId, Int32 UnderId, string Pin, string Mobile, string Phone, string Remark, string SyncId, bool Active, string BlockReason,
  int PartyType, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal CreditLimit, decimal OutStanding, int UserId, string DOA, string DOB, string Email, string Imgurl, string GSTINNo, int SMID, string Partygstin, string compcode, string TCountryName, string TStateName, int countryid, int regionid, int distictid, int citytypeid, int cityconveyancetype, int stateid, int SuperstockistID)
       {

           DbParameter[] dbParam = new DbParameter[40];
           dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, PartyName);
           dbParam[1] = new DbParameter("@DistributorName", DbParameter.DbType.VarChar, 150, Distributor);
           dbParam[2] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
           dbParam[3] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
           dbParam[4] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
           dbParam[5] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
           dbParam[6] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
           dbParam[7] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
           dbParam[8] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
           dbParam[9] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
           dbParam[10] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
           dbParam[11] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
           dbParam[12] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
           dbParam[13] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
           dbParam[14] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
           dbParam[15] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);

           dbParam[16] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
           dbParam[17] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);

           if (!Active)
           {
               dbParam[18] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
               dbParam[19] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
               dbParam[20] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
           }
           else
           {
               dbParam[18] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
               dbParam[19] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
               dbParam[20] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
           }
           dbParam[21] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);

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
           dbParam[26] = new DbParameter("@AreaID", DbParameter.DbType.Int, 10, AreaId);
           dbParam[27] = new DbParameter("@BeatId", DbParameter.DbType.Int, 10, BeatId);
           dbParam[28] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);
           dbParam[29] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
           dbParam[30] = new DbParameter("@TCountryName", DbParameter.DbType.VarChar, 100, TCountryName);
           dbParam[31] = new DbParameter("@TStateName", DbParameter.DbType.VarChar, 100, TStateName);
           dbParam[32] = new DbParameter("@countryid", DbParameter.DbType.Int, 10, countryid);
           dbParam[33] = new DbParameter("@regionid", DbParameter.DbType.Int, 10, regionid);
           dbParam[34] = new DbParameter("@distictid", DbParameter.DbType.Int, 10, distictid);

           dbParam[35] = new DbParameter("@CreatedUserId", DbParameter.DbType.Int, 10, UserId);
           dbParam[36] = new DbParameter("@citytypeid", DbParameter.DbType.Int, 10, citytypeid);
           dbParam[37] = new DbParameter("@cityconveyancetype", DbParameter.DbType.Int, 10, cityconveyancetype);
           dbParam[38] = new DbParameter("@stateid", DbParameter.DbType.Int, 10, stateid);
           dbParam[39] = new DbParameter("@UnderId", DbParameter.DbType.Int, 10, SuperstockistID);
           //"Sp_InsertPartyForm"
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertRetailerForm_Tally", dbParam);
           return Convert.ToInt32(dbParam[23].Value);
       }



       public int UpdateParty_Tally(string PartyName, string Distributor, string Address1, string Address2, Int32 CityId, Int32 AreaId, Int32 BeatId, Int32 UnderId, string Pin, string Mobile, string Phone, string Remark, string SyncId, bool Active, string BlockReason,
      int PartyType, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal CreditLimit, decimal OutStanding, int UserId, string DOA, string DOB, string Email, string Imgurl, string GSTINNo, int SMID, string Partygstin, string compcode, string TCountryName, string TStateName, int countryid, int regionid, int distictid, int citytypeid, int cityconveyancetype, int stateid, int SuperstockistID)
       {

           DbParameter[] dbParam = new DbParameter[40];
           dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, PartyName);
           dbParam[1] = new DbParameter("@DistributorName", DbParameter.DbType.VarChar, 150, Distributor);
           dbParam[2] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
           dbParam[3] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
           dbParam[4] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
           dbParam[5] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
           dbParam[6] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
           dbParam[7] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
           dbParam[8] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
           dbParam[9] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
           dbParam[10] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
           dbParam[11] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
           dbParam[12] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
           dbParam[13] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
           dbParam[14] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
           dbParam[15] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);

           dbParam[16] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
           dbParam[17] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);

           if (!Active)
           {
               dbParam[18] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
               dbParam[19] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
               dbParam[20] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
           }
           else
           {
               dbParam[18] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
               dbParam[19] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
               dbParam[20] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
           }
           dbParam[21] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);

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
           dbParam[26] = new DbParameter("@AreaID", DbParameter.DbType.Int, 10, AreaId);
           dbParam[27] = new DbParameter("@BeatId", DbParameter.DbType.Int, 10, BeatId);
           dbParam[28] = new DbParameter("@Partygstin", DbParameter.DbType.VarChar, 50, Partygstin);
           dbParam[29] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
           dbParam[30] = new DbParameter("@TCountryName", DbParameter.DbType.VarChar, 100, TCountryName);
           dbParam[31] = new DbParameter("@TStateName", DbParameter.DbType.VarChar, 100, TStateName);
           dbParam[32] = new DbParameter("@countryid", DbParameter.DbType.Int, 10, countryid);
           dbParam[33] = new DbParameter("@regionid", DbParameter.DbType.Int, 10, regionid);
           dbParam[34] = new DbParameter("@distictid", DbParameter.DbType.Int, 10, distictid);

           dbParam[35] = new DbParameter("@CreatedUserId", DbParameter.DbType.Int, 10, UserId);
           dbParam[36] = new DbParameter("@citytypeid", DbParameter.DbType.Int, 10, citytypeid);
           dbParam[37] = new DbParameter("@cityconveyancetype", DbParameter.DbType.Int, 10, cityconveyancetype);
           dbParam[38] = new DbParameter("@stateid", DbParameter.DbType.Int, 10, stateid);
           dbParam[39] = new DbParameter("@UnderId", DbParameter.DbType.Int, 10, SuperstockistID);
           //"Sp_InsertPartyForm"
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdateRetailerForm_Tally", dbParam);
           return Convert.ToInt32(dbParam[23].Value);
       }

       public int insertRetailersaleinvheader_Tally(string Docid, string VDate, decimal taxamt, decimal Billamount, decimal Roundoff, decimal Disc, string SyncId, string compcode = "")
       {

           DbParameter[] dbParam = new DbParameter[9];


           dbParam[0] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, Docid);
           dbParam[1] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(VDate));
           dbParam[2] = new DbParameter("@taxamt", DbParameter.DbType.Decimal, 20, taxamt);
           dbParam[3] = new DbParameter("@Billamount", DbParameter.DbType.Decimal, 20, Billamount);
           dbParam[4] = new DbParameter("@Roundoff", DbParameter.DbType.Decimal, 20, Roundoff);
           dbParam[5] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 150, SyncId);
           dbParam[6] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, compcode);
           dbParam[7] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           dbParam[8] = new DbParameter("@Discount", DbParameter.DbType.Decimal, 20, Disc);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertRetailersaleinvoiceheader_Tally", dbParam);
           return Convert.ToInt32(dbParam[7].Value);
       }
       public int insertRetailersaleinvdetail_Tally(int DistInvId, int sno, string Docid, string VDate, decimal taxamt, decimal Qty, decimal Rate, decimal Amount, string SyncId, string itemname, int ItemMasterid, string Unit, string compcode = "")
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
           dbParam[12] = new DbParameter("@ItemId", DbParameter.DbType.Int, 1, ItemMasterid);
           dbParam[13] = new DbParameter("@Unit", DbParameter.DbType.VarChar, 20, Unit);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertRetailesaleinvoicedetail_Tally", dbParam);
           return Convert.ToInt32(dbParam[11].Value);
       }

       public int insertsaleinvexpensedetail_Tally(string docid, string description, decimal Amount, string Compcode)
       {

           DbParameter[] dbParam = new DbParameter[5];


           dbParam[0] = new DbParameter("@Docid", DbParameter.DbType.VarChar, 30, docid);
           dbParam[1] = new DbParameter("@description", DbParameter.DbType.VarChar, 150, description);
           dbParam[2] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 20, Amount);


           dbParam[3] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           dbParam[4] = new DbParameter("@Compcode", DbParameter.DbType.VarChar, 50, Compcode);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_insertRetailersaleinvexpensedetail_Tally", dbParam);
           return Convert.ToInt32(dbParam[3].Value);
       }

       //v2--- jyoti mam 17-07-2021

       public int UpdatePartyMobileForApproval(Int32 PartyId, string PartyName, string Address1, string Address2, Int32 CityId, Int32 AreaId, Int32 BeatId, Int32 UnderId, string Pin, string Mobile, string Phone, string Remark, string SyncId, string IndId, decimal Potential, bool Active, string BlockReason,
int PartyType, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, int UserId, string DOA, string DOB, string Email, string Imgurl,
   string GSTINNo, Int32 Radius, string AppStatus, Int32 AppUserId, string AppRemark, Int32 AppSMId, bool Isblock, string AppBlockRemark, string AppBlockStatus, Int32 AppBlockby = 0)
       {

           DbParameter[] dbParam = new DbParameter[41];
           dbParam[0] = new DbParameter("@PartyId", DbParameter.DbType.Int, 10, PartyId);
           dbParam[1] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, PartyName);
           dbParam[2] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
           dbParam[3] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
           dbParam[4] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
           dbParam[5] = new DbParameter("@AreaId", DbParameter.DbType.Int, 10, AreaId);
           dbParam[6] = new DbParameter("@BeatId", DbParameter.DbType.Int, 10, BeatId);
           dbParam[7] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
           dbParam[8] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
           dbParam[9] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
           dbParam[10] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
           dbParam[11] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
           dbParam[12] = new DbParameter("@IndId", DbParameter.DbType.Int, 10, IndId);
           dbParam[13] = new DbParameter("@Potential", DbParameter.DbType.Decimal, 10, Potential);
           dbParam[14] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
           dbParam[15] = new DbParameter("@UnderID", DbParameter.DbType.Int, 10, UnderId);
           dbParam[16] = new DbParameter("@PartyType", DbParameter.DbType.Int, 10, PartyType);
           dbParam[17] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
           dbParam[18] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
           dbParam[19] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
           dbParam[20] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
           dbParam[21] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
           if (BlockReason != "")
           {
               dbParam[22] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
               dbParam[23] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 40, DateTime.Now.ToUniversalTime().AddSeconds(19800));
               dbParam[24] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
           }
           else
           {
               dbParam[22] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
               dbParam[23] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
               dbParam[24] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
           }
           dbParam[25] = new DbParameter("@CreatedUserId", DbParameter.DbType.Int, 10, UserId);
           dbParam[26] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           if (DOA != "")
           {
               dbParam[27] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
           }
           else
           {
               dbParam[27] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }

           if (DOB != "")
           {
               dbParam[28] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
           }
           else
           {
               dbParam[28] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }
           dbParam[29] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
           dbParam[30] = new DbParameter("@Imgurl", DbParameter.DbType.VarChar, 300, Imgurl);
           dbParam[31] = new DbParameter("@GSTINNo", DbParameter.DbType.VarChar, 50, GSTINNo);
           if (Radius == 0)
           {
               dbParam[32] = new DbParameter("@Radius", DbParameter.DbType.Int, 10, DBNull.Value);
           }
           else
           {
               dbParam[32] = new DbParameter("@Radius", DbParameter.DbType.Int, 10, Radius);
           }

           dbParam[33] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 15, AppStatus);
           dbParam[34] = new DbParameter("@AppUserId", DbParameter.DbType.Int, 10, AppUserId);
           dbParam[35] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 500, AppRemark);
           dbParam[36] = new DbParameter("@AppSMId", DbParameter.DbType.Int, 10, AppSMId);


           if (AppBlockby == 0)
           {
               dbParam[37] = new DbParameter("@Isblock", DbParameter.DbType.Bit, 1, DBNull.Value);
               dbParam[38] = new DbParameter("@AppBlockby", DbParameter.DbType.Int, 10, DBNull.Value);
               dbParam[39] = new DbParameter("@AppBlockRemark", DbParameter.DbType.VarChar, 500, DBNull.Value);

               dbParam[40] = new DbParameter("@AppBlockStatus", DbParameter.DbType.VarChar, 15, DBNull.Value);

           }
           else
           {


               dbParam[37] = new DbParameter("@Isblock", DbParameter.DbType.Bit, 1, Isblock);
               dbParam[38] = new DbParameter("@AppBlockby", DbParameter.DbType.Int, 10, AppBlockby);
               dbParam[39] = new DbParameter("@AppBlockRemark", DbParameter.DbType.VarChar, 500, AppBlockRemark);

               dbParam[40] = new DbParameter("@AppBlockStatus", DbParameter.DbType.VarChar, 15, AppBlockStatus);
           }
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdatePartyFromManagerApp", dbParam);
           return Convert.ToInt32(dbParam[26].Value);
       }
    }
    
}
