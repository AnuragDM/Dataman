using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BAL.Department
{
    public class deptAll
    {
        public int Insert(string DepName, string SyncId, bool Active, DateTime CreatedDate)
        {
            DbParameter[] dbParam = new DbParameter[7];
            dbParam[0] = new DbParameter("@DepId", DbParameter.DbType.Int, 1, 0);
            dbParam[1] = new DbParameter("@DepName", DbParameter.DbType.VarChar, 100, DepName);
            dbParam[2] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[3] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[4] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[5] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[6] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastDepartment_ups", dbParam);
            return Convert.ToInt32(dbParam[6].Value);
        }

        public int Update(string DepId, string DepName, string SyncId, bool Active, DateTime CreatedDate)
        {
            DbParameter[] dbParam = new DbParameter[7];
            dbParam[0] = new DbParameter("@DepId", DbParameter.DbType.Int, 1, Convert.ToInt32(DepId));
            dbParam[1] = new DbParameter("@DepName", DbParameter.DbType.VarChar, 100, DepName);
            dbParam[2] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[3] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[4] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[5] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[6] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastDepartment_ups", dbParam);
            return Convert.ToInt32(dbParam[6].Value);
        }

        public DataTable GetDeptData(int DepId)
        {
            DbParameter[] dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@DepId", DbParameter.DbType.Int, 1, DepId);
           // DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_MastDepartment_sel", dbParam);
            return DbConnectionDAL.GetDataTable(CommandType.StoredProcedure, "sp_MastDepartment_sel", dbParam);
        }

        public int delete(string DepId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@DepId", DbParameter.DbType.Int, 1, Convert.ToInt32(DepId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastDepartment_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
