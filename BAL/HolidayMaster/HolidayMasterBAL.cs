using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BAL.HolidayMaster
{
    public class HolidayMasterBAL
    {
        public int Insert(string strDate, string description, string GeoType, string GeoName,string SyncId, bool Active, DateTime CreatedDate)
        {    
            DbParameter[] dbParam = new DbParameter[10];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, 0);
            dbParam[1] = new DbParameter("@holiday_date", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(strDate));
            dbParam[2] = new DbParameter("@description", DbParameter.DbType.VarChar, 50, description);
            dbParam[3] = new DbParameter("@Area_id", DbParameter.DbType.Int, 4, Convert.ToInt32(GeoName));
            dbParam[4] = new DbParameter("@GeoTypeId", DbParameter.DbType.Int, 4, Convert.ToInt32(GeoType));
            dbParam[5] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[6] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[7] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[8] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[9] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastHoliday_ups", dbParam);
            return Convert.ToInt32(dbParam[9].Value);
        }

        public int Update(string Id, string strDate, string description, string GeoType, string GeoName, string SyncId, bool Active, DateTime CreatedDate)
        {
            DbParameter[] dbParam = new DbParameter[10];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, Id);
            dbParam[1] = new DbParameter("@holiday_date", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(strDate));
            dbParam[2] = new DbParameter("@description", DbParameter.DbType.VarChar, 50, description);
            dbParam[3] = new DbParameter("@Area_id", DbParameter.DbType.Int, 4, Convert.ToInt32(GeoName));
            dbParam[4] = new DbParameter("@GeoTypeId", DbParameter.DbType.Int, 4, Convert.ToInt32(GeoType));
            dbParam[5] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[6] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[7] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[8] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[9] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastHoliday_ups", dbParam);
            return Convert.ToInt32(dbParam[9].Value);
        }


        public int delete(string Id)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, Convert.ToInt32(Id));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastHoliday_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
