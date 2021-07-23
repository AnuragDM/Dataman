using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BAL.DesignationMaster
{
    public class DesignationBAL
    {
        public int Insert(string DesName, string SyncId, bool Active, DateTime CreatedDate, bool EligibleForConveyance, int GradeId)
        {
            DbParameter[] dbParam = new DbParameter[9];
            dbParam[0] = new DbParameter("@DesId", DbParameter.DbType.Int, 1, 0);
            dbParam[1] = new DbParameter("@DesName", DbParameter.DbType.VarChar, 100, DesName);
            dbParam[2] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[3] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[4] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[5] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[6] = new DbParameter("@EligibleForConveyance", DbParameter.DbType.Bit, 1, EligibleForConveyance);
            dbParam[7] = new DbParameter("@GradeId", DbParameter.DbType.Int, 10, GradeId);
            dbParam[8] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastDesignation_ups", dbParam);
            return Convert.ToInt32(dbParam[8].Value);
        }

        public int Update(string DesId, string DesName, string SyncId, bool Active, DateTime CreatedDate,bool EligibleForConveyance,int GradeId)
        {
            DbParameter[] dbParam = new DbParameter[9];
            dbParam[0] = new DbParameter("@DesId", DbParameter.DbType.Int, 1, Convert.ToInt32(DesId));
            dbParam[1] = new DbParameter("@DesName", DbParameter.DbType.VarChar, 100, DesName);
            dbParam[2] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[3] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[4] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[5] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[6] = new DbParameter("@EligibleForConveyance", DbParameter.DbType.Bit, 1, EligibleForConveyance);
            dbParam[7] = new DbParameter("@GradeId", DbParameter.DbType.Int, 10, GradeId);
            dbParam[8] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastDesignation_ups", dbParam);
            return Convert.ToInt32(dbParam[8].Value);
        }
      

        public int delete(string DesId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@DesId", DbParameter.DbType.Int, 1, Convert.ToInt32(DesId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastDesignation_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
