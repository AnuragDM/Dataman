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
    }
    
}
