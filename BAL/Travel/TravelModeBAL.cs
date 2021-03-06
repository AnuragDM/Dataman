using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;
using System;

namespace BAL
{
   public class TravelModeBAL
    {
       public int InsertUpdateTravelMode(Int32 Id, string Name, decimal PerKM, string Remarks, string SyncId, bool Active, bool chktravelmode)
        {
            DbParameter[] dbParam = new DbParameter[8];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Id);
            dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 30, Name);
            dbParam[2] = new DbParameter("@PerKM", DbParameter.DbType.Decimal, 15, PerKM);
            dbParam[3] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 500, Remarks);
            dbParam[4] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 20, SyncId);
            dbParam[5] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[6] = new DbParameter("@chktravelmode", DbParameter.DbType.Bit, 1, chktravelmode);
            dbParam[7] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsUpdTravelMode", dbParam);
            return Convert.ToInt32(dbParam[7].Value);
        }
        public int delete(string Id)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteTravelMode", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
