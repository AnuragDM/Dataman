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
   public class SalesPersonsBAL
   {
       public int InsertSalespersons(string SMName, string Pin, string SalesPerType, string DeviceNo, string DSRAllowDays, string isAdmin, string Address1, string Address2,
          string CityId, string Email, string Mobile, string Remarks, string RoleId, Int32 UserId, string SyncId, string UnderId, int GradeId, int DeptId, int DesgId, string DOB, string DOA, string ResCenID, string BlockReason, int BlockBy, string EmpName, bool AllowChangeCity, int MeetAllowDays,bool MobileAccess)
       {
           DbParameter[] dbParam = new DbParameter[30];
           dbParam[0] = new DbParameter("@SMName", DbParameter.DbType.VarChar, 50, SMName);
           dbParam[1] = new DbParameter("@SalesPerType", DbParameter.DbType.VarChar, 50, SalesPerType);
           dbParam[2] = new DbParameter("@UnderId", DbParameter.DbType.Int, 10, UnderId);
           dbParam[3] = new DbParameter("@DeptId", DbParameter.DbType.Int, 10, DeptId);
           dbParam[4] = new DbParameter("@DesgId", DbParameter.DbType.Int, 10, DesgId);
           dbParam[5] = new DbParameter("@GradeId", DbParameter.DbType.Int, 10, DBNull.Value);
           dbParam[6] = new DbParameter("@ResCenID", DbParameter.DbType.Int,10, 1);
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
           dbParam[17] = new DbParameter("@SyncId", DbParameter.DbType.VarChar,50, SyncId);
           dbParam[18] = new DbParameter("@DSRAllowDays", DbParameter.DbType.Int, 10, DSRAllowDays);
           dbParam[19] = new DbParameter("@isAdmin", DbParameter.DbType.VarChar, 1, isAdmin);
           dbParam[20] = new DbParameter("@UserId", DbParameter.DbType.Int, 10, UserId);
           dbParam[21] = new DbParameter("@RoleId", DbParameter.DbType.Int, 10, RoleId);
           if (isAdmin == "1")
           {
               dbParam[22] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 15, DBNull.Value);
               dbParam[23] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200, DBNull.Value);
               dbParam[24] = new DbParameter("@BlockBy", DbParameter.DbType.Int,10, DBNull.Value);
           }
           else
           {
               dbParam[22] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 15, DateTime.Now.ToUniversalTime().AddSeconds(19800));
               dbParam[23] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 200,BlockReason);
               dbParam[24] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10, BlockBy);
           }
           dbParam[25] = new DbParameter("@EmpName", DbParameter.DbType.VarChar, 50, EmpName);
           dbParam[26] = new DbParameter("@AllowChangeCity", DbParameter.DbType.Bit, 1, AllowChangeCity);           
           dbParam[27] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           dbParam[28] = new DbParameter("@MeetAllowDays", DbParameter.DbType.Int, 3, MeetAllowDays);
           dbParam[29] = new DbParameter("@MobileAccess", DbParameter.DbType.Bit, 1, MobileAccess);   

           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertSalesRepForm", dbParam);
           return Convert.ToInt32(dbParam[27].Value);
       }
       public int UpdateSalespersons(Int32 SMID, string SMName, string Pin, string SalesPerType, string DeviceNo, string DSRAllowDays, string isAdmin, string Address1, string Address2,
       string CityId, string Email, string Mobile, string Remarks, string RoleId, Int32 UserId, string SyncId, string UnderId, int GradeId, int DeptId, int DesgId, string DOB, string DOA, string ResCenID, string BlockReason, int BlockBy, string EmpName, bool AllowChangeCity, int MeetAllowDays, bool MobileAccess)
       {
           DbParameter[] dbParam = new DbParameter[31];
           dbParam[0] = new DbParameter("@SMId", DbParameter.DbType.VarChar, 50, SMID);
           dbParam[1] = new DbParameter("@SMName", DbParameter.DbType.VarChar, 50, SMName);
           dbParam[2] = new DbParameter("@SalesPerType", DbParameter.DbType.VarChar, 50, SalesPerType);
           dbParam[3] = new DbParameter("@UnderId", DbParameter.DbType.Int, 10, UnderId);
           dbParam[4] = new DbParameter("@DeptId", DbParameter.DbType.Int, 10, DeptId);
           dbParam[5] = new DbParameter("@DesgId", DbParameter.DbType.Int, 10, DesgId);
           dbParam[6] = new DbParameter("@GradeId", DbParameter.DbType.Int, 10, DBNull.Value);
           dbParam[7] = new DbParameter("@ResCenID", DbParameter.DbType.Int, 10, 1);
           if (!string.IsNullOrEmpty(DOB))
               dbParam[8] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DOB);
           else
               dbParam[8] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 12, DBNull.Value);
           if (!string.IsNullOrEmpty(DOA))
               dbParam[9] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DOA);
           else
               dbParam[9] = new DbParameter("@DOA", DbParameter.DbType.DateTime, 12, DBNull.Value);
           dbParam[10] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
           dbParam[11] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
           dbParam[12] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
           dbParam[13] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
           dbParam[14] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 15, Mobile);
           dbParam[15] = new DbParameter("@DeviceNo", DbParameter.DbType.VarChar, 50, DeviceNo);
           dbParam[16] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
           dbParam[17] = new DbParameter("@Remarks", DbParameter.DbType.Text, 1000, Remarks);
           dbParam[18] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
           dbParam[19] = new DbParameter("@DSRAllowDays", DbParameter.DbType.Int, 10, DSRAllowDays);
           dbParam[20] = new DbParameter("@isAdmin", DbParameter.DbType.VarChar, 1, isAdmin);
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
               dbParam[24] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 10,BlockBy);
           }
           dbParam[25] = new DbParameter("@UserId", DbParameter.DbType.Int, 10, UserId);
           dbParam[26] = new DbParameter("@EmpName", DbParameter.DbType.VarChar, 50, EmpName);
           dbParam[27] = new DbParameter("@AllowChangeCity", DbParameter.DbType.Bit, 1, AllowChangeCity);
           dbParam[28] = new DbParameter("@MeetAllowDays", DbParameter.DbType.Int, 3, MeetAllowDays);
           dbParam[29] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           dbParam[30] = new DbParameter("@MobileAccess", DbParameter.DbType.Bit, 1, MobileAccess);   

           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdateSalesRepForm", dbParam);
           return Convert.ToInt32(dbParam[29].Value);
       }
       public int delete(string SMId)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@SMId", DbParameter.DbType.Int, 10, Convert.ToInt32(SMId));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteMastSalesRep", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
    }
}
