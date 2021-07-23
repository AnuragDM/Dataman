using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;
using System;


namespace BAL
{
  public class LocalLodgingBoardingLimitBAL
  {
      public int InsertUpdateLodgingBoardingLimit(Int32 Id, Int32 CityTypeId, Int32 DesId, decimal Amount, string SyncId, bool Active, string Remarks)
      {
          DbParameter[] dbParam = new DbParameter[8];
          dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Id);
          dbParam[1] = new DbParameter("@CityTypeId", DbParameter.DbType.Int, 10, CityTypeId);
          dbParam[2] = new DbParameter("@DesId", DbParameter.DbType.Int, 10, DesId);
          dbParam[3] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 15, Amount);
          dbParam[4] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 20, SyncId);
          dbParam[5] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
          dbParam[6] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 500, Remarks);
          dbParam[7] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsUpdLocalLodgingBoardingLimit", dbParam);
          return Convert.ToInt32(dbParam[7].Value);
      }
      public int delete(string Id)
      {
          DbParameter[] dbParam = new DbParameter[2];
          dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
          dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteLocalLodgingBoardingLimit", dbParam);
          return Convert.ToInt32(dbParam[1].Value);
      }
    }
}
