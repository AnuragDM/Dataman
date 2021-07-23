using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;
using System;

namespace BAL
{
   public class CityTypeBAL
   {
       public int InsertUpdateCityType(Int32 Id, string Name)
       {
           DbParameter[] dbParam = new DbParameter[3];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Id);
           dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 30, Name);
           dbParam[2] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsUpdCityType", dbParam);
           return Convert.ToInt32(dbParam[2].Value);
       }
       public int delete(string Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteCityType", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
    }
}
