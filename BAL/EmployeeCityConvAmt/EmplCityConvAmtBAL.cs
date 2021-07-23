using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;
using System;

namespace BAL
{
   public class EmplCityConvAmtBAL
   {
       public int Insert(Int32 SMId,Int32 CityId,decimal ConvAmt)
       {
           DbParameter[] dbParam = new DbParameter[4];
           dbParam[0] = new DbParameter("@SMId", DbParameter.DbType.Int, 10, SMId);
           dbParam[1] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
           dbParam[2] = new DbParameter("@ConvAmt", DbParameter.DbType.Decimal, 20, ConvAmt);
           dbParam[3] = new DbParameter("@OutputParam", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertMastEmployeeCityConvLimit", dbParam);
           return Convert.ToInt32(dbParam[3].Value);
       }
       public int delete(string Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteExpenseType", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
    }
}
