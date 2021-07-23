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
   public class DistributorBAL
   {
       public int InsertDistributors(string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, string UserName, bool Active, string Phone,
          int RoleID, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal OpenOrder, decimal CreditLimit, decimal OutStanding, int CreditDays, int UserId, string Telex,string Fax,string Distributor2,Int32 SMID,string DOA,string DOB, int Areid)
       {
           DbParameter[] dbParam = new DbParameter[33];
           dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, DistName);
           dbParam[1] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
           dbParam[2] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
           dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
           dbParam[4] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
           dbParam[5] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
           dbParam[6] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
           dbParam[7] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
           dbParam[8] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
           dbParam[9] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
           dbParam[10] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
           dbParam[11] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
           dbParam[12] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
           dbParam[13] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
           dbParam[14] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
           dbParam[15] = new DbParameter("@OpenOrder", DbParameter.DbType.Decimal, 20, OpenOrder);
           dbParam[16] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
           dbParam[17] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);
           dbParam[18] = new DbParameter("@CreditDays", DbParameter.DbType.Int, 10, CreditDays);
           if (!Active)
           {
               dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10,UserId);
               dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
               dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar,200, BlockReason);
           }
           else
           {
               dbParam[19] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
               dbParam[20] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
               dbParam[21] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
           }
           dbParam[22] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
           dbParam[23] = new DbParameter("@UserName", DbParameter.DbType.VarChar, 50, UserName);
           dbParam[24] = new DbParameter("@RoleID", DbParameter.DbType.Int, 10, RoleID);
           dbParam[25] = new DbParameter("@Telex", DbParameter.DbType.VarChar,50, RoleID);
           dbParam[26] = new DbParameter("@Fax", DbParameter.DbType.VarChar, 10, Fax);
           dbParam[27] = new DbParameter("@DistributorName2", DbParameter.DbType.VarChar, 100, Distributor2);
           dbParam[28] = new DbParameter("@SMID", DbParameter.DbType.Int, 10, SMID);
        
           dbParam[29] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           if (DOA != "")
           {
               dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
           }
           else
           {
               dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }

           if (DOB != "")
           {
               dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
           }
           else
           {
               dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }
           dbParam[32] = new DbParameter("@AreaID", DbParameter.DbType.Int, 10, Areid);

           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertDistributorForm", dbParam);
           return Convert.ToInt32(dbParam[29].Value);
       }
       public int UpdateDistributors(Int32 DistId, string DistName, string Address1, string Address2, string CityId, string Pin, string Email, string Mobile, string Remark, string SyncId, string BlockReason, string UserName, bool Active, string Phone,
         int RoleID, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, decimal OpenOrder, decimal CreditLimit, decimal OutStanding, int CreditDays, int UserId, string Telex, string Fax, string Distributor2, Int32 SMID, string DOA, string DOB, int AreaID)
       {
           DbParameter[] dbParam = new DbParameter[33];
           dbParam[0] = new DbParameter("@DistId", DbParameter.DbType.Int, 10, DistId);
           dbParam[1] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, DistName);
           dbParam[2] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
           dbParam[3] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
           dbParam[4] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
           dbParam[5] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
           dbParam[6] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
           dbParam[7] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 20, Phone);
           dbParam[8] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 300, Remark);
           dbParam[9] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
           dbParam[10] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
           dbParam[11] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 100, ContactPerson);
           dbParam[12] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 50, CSTNo);
           dbParam[13] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 50, VatTin);
           dbParam[14] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 50, ServiceTax);
           dbParam[15] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 50, PanNo);
           dbParam[16] = new DbParameter("@OpenOrder", DbParameter.DbType.Decimal, 20, OpenOrder);
           dbParam[17] = new DbParameter("@CreditLimit", DbParameter.DbType.Decimal, 20, CreditLimit);
           dbParam[18] = new DbParameter("@OutStanding", DbParameter.DbType.Decimal, 20, OutStanding);
           dbParam[19] = new DbParameter("@CreditDays", DbParameter.DbType.Int, 10, CreditDays);
           if (!Active)
           {
               dbParam[20] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, UserId);
               dbParam[21] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DateTime.Now.ToUniversalTime().AddSeconds(19800));
               dbParam[22] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, BlockReason);
           }
           else
           {
               dbParam[20] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, DBNull.Value);
               dbParam[21] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 12, DBNull.Value);
               dbParam[22] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
           }
           dbParam[23] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
           dbParam[24] = new DbParameter("@RoleID", DbParameter.DbType.Int, 10, RoleID);
           dbParam[25] = new DbParameter("@Telex", DbParameter.DbType.VarChar, 50, Telex);
           dbParam[26] = new DbParameter("@Fax", DbParameter.DbType.VarChar, 10, Fax);
           dbParam[27] = new DbParameter("@DistributorName2", DbParameter.DbType.VarChar, 100, Distributor2);
           dbParam[28] = new DbParameter("@SMID", DbParameter.DbType.Int, 10, SMID);
           dbParam[29] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           if (DOA != "")
           {
               dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOA));
           }
           else
           {
               dbParam[30] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }

           if (DOB != "")
           {
               dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, Convert.ToDateTime(DOB));
           }
           else
           {
               dbParam[31] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
           }

           dbParam[32] = new DbParameter("@AreaID", DbParameter.DbType.Int, 10, AreaID);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdateDistributorForm", dbParam);
           return Convert.ToInt32(dbParam[29].Value);
       }
       public int delete(string PartyId)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@DistID", DbParameter.DbType.Int, 1, Convert.ToInt32(PartyId));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteDistributors", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
       public int InsertDItemStock(string STKDocId, int UserId, string VDate, int VisitId, int SMId, int DistId, string DistCode, int AreaId, int ItemId, decimal Qty, string Android_Id, string Created_date,decimal unit,decimal cases)
       {
           DbParameter[] dbParam = new DbParameter[17];
           dbParam[0] = new DbParameter("@STKId", DbParameter.DbType.Int, 4, 0);
           dbParam[1] = new DbParameter("@STKDocId", DbParameter.DbType.VarChar, 30, STKDocId);
           dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
           dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
           dbParam[4] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
           dbParam[5] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
           dbParam[6] = new DbParameter("@DistCode", DbParameter.DbType.VarChar, 50, DistCode);
           dbParam[7] = new DbParameter("@AreaId", DbParameter.DbType.Int, 4, AreaId);
           dbParam[8] = new DbParameter("@ItemId", DbParameter.DbType.Int, 4, ItemId);
           dbParam[9] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 20, Qty);
           dbParam[10] = new DbParameter("@Android_Id", DbParameter.DbType.VarChar, 150, Android_Id);
           dbParam[11] = new DbParameter("@Created_date", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(Created_date));
           dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
           dbParam[13] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisitId);
           dbParam[14] = new DbParameter("@unit", DbParameter.DbType.Decimal, 20, unit);
           dbParam[15] = new DbParameter("@cases", DbParameter.DbType.Decimal, 20, cases);
           dbParam[16] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransDistStock_Ins", dbParam);
           return Convert.ToInt32(dbParam[16].Value);
       }
       public int InsertDItemStock(int STKId, string STKDocId, int UserId, string VDate, int SMId, int DistId, string DistCode, int AreaId, int ItemId, decimal Qty, string Android_Id, string Created_date)
       {
           int visId = 0;
           DbParameter[] dbParam = new DbParameter[15];
           dbParam[0] = new DbParameter("@STKId", DbParameter.DbType.Int, 4, STKId);
           dbParam[1] = new DbParameter("@STKDocId", DbParameter.DbType.VarChar, 30, STKDocId);
           dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
           dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
           dbParam[4] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
           dbParam[5] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
           dbParam[6] = new DbParameter("@DistCode", DbParameter.DbType.VarChar, 150, DistCode);
           dbParam[7] = new DbParameter("@AreaId", DbParameter.DbType.Int, 4, AreaId);
           dbParam[8] = new DbParameter("@ItemId", DbParameter.DbType.Int, 4, ItemId);
           dbParam[9] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 20, Qty);
           dbParam[10] = new DbParameter("@Android_Id", DbParameter.DbType.VarChar, 150, Android_Id);
           dbParam[11] = new DbParameter("@Created_date", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(Created_date));
           dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
           dbParam[13] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, visId);
           dbParam[14] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransDistStock_Ins", dbParam);
           return Convert.ToInt32(dbParam[14].Value);
       }
       public int delete(int Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransDistStock_Del", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
    }
}
