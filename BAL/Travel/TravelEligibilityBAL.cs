using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;
using System;

namespace BAL
{
   public class TravelEligibilityBAL
   {
       public int InsertUpdateTravelEligibility(Int32 Id, Int32 TravelModeId, Int32 DesId, string SyncId, bool Active)
       {
           DbParameter[] dbParam = new DbParameter[6];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Id);
           dbParam[1] = new DbParameter("@TravelModeId", DbParameter.DbType.Int, 10, TravelModeId);
           dbParam[2] = new DbParameter("@DesId", DbParameter.DbType.Int, 10, DesId);
           dbParam[3] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 20, SyncId);
           dbParam[4] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
           dbParam[5] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsUpdTravelEligibility", dbParam);
           return Convert.ToInt32(dbParam[5].Value);
       }
       public int delete(string Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteTravelEligibility", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
    }
}
