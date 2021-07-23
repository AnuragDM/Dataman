using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BAL.FailedVisitRemarks
{
    public class FailedVisitRemarksBAL
    {
        public int Insert(string FVName, string SyncId, bool Active, DateTime CreatedDate)
        {
            DbParameter[] dbParam = new DbParameter[7];
            dbParam[0] = new DbParameter("@FVId", DbParameter.DbType.Int, 1, 0);
            dbParam[1] = new DbParameter("@FVName", DbParameter.DbType.VarChar, 100, FVName);
            dbParam[2] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[3] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[4] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[5] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[6] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastFailedVisitRemark_ups", dbParam);
            return Convert.ToInt32(dbParam[6].Value);
        }

        public int Update(string FVId, string FVName, string SyncId, bool Active, DateTime CreatedDate)
        {
            DbParameter[] dbParam = new DbParameter[7];
            dbParam[0] = new DbParameter("@FVId", DbParameter.DbType.Int, 1, Convert.ToInt32(FVId));
            dbParam[1] = new DbParameter("@FVName", DbParameter.DbType.VarChar, 100, FVName);
            dbParam[2] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[3] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[4] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[5] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[6] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastFailedVisitRemark_ups", dbParam);
            return Convert.ToInt32(dbParam[6].Value);
        }


        public int delete(string FVId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@FVId", DbParameter.DbType.Int, 1, Convert.ToInt32(FVId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastFailedVisitRemark_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
