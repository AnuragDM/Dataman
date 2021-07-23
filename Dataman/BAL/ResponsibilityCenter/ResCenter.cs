using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;


namespace BAL.ResponsibilityCenter
{
    public class ResCenter
    {
        public int Insert(string ResCenName, string PlantCode, string PlantType, string OrderType, string DivisionCode, string SyncId, bool Active, DateTime CreatedDate)
        {
            DbParameter[] dbParam = new DbParameter[11];
            dbParam[0] = new DbParameter("@ResCenId", DbParameter.DbType.Int, 1, 0);
            dbParam[1] = new DbParameter("@ResCenName", DbParameter.DbType.VarChar, 100, ResCenName);
            dbParam[2] = new DbParameter("@PlantCode", DbParameter.DbType.VarChar, 50, PlantCode);
            dbParam[3] = new DbParameter("@PlantType", DbParameter.DbType.VarChar, 1, PlantType);
            dbParam[4] = new DbParameter("@OrderType", DbParameter.DbType.VarChar, 50, OrderType);
            dbParam[5] = new DbParameter("@DivisionCode", DbParameter.DbType.Int, 4, DivisionCode);           
            dbParam[6] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[7] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[8] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[9] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[10] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastResCenter_ups", dbParam);
            return Convert.ToInt32(dbParam[10].Value);
        }

        public int Update(string ResCenId, string ResCenName, string PlantCode, string PlantType, string OrderType, string DivisionCode, string SyncId, bool Active, DateTime CreatedDate)
        {
            DbParameter[] dbParam = new DbParameter[11];
            dbParam[0] = new DbParameter("@ResCenId", DbParameter.DbType.Int, 1, ResCenId);
            dbParam[1] = new DbParameter("@ResCenName", DbParameter.DbType.VarChar, 100, ResCenName);
            dbParam[2] = new DbParameter("@PlantCode", DbParameter.DbType.VarChar, 50, PlantCode);
            dbParam[3] = new DbParameter("@PlantType", DbParameter.DbType.VarChar, 1, PlantType);
            dbParam[4] = new DbParameter("@OrderType", DbParameter.DbType.VarChar, 50, OrderType);
            dbParam[5] = new DbParameter("@DivisionCode", DbParameter.DbType.Int, 4, DivisionCode);
            dbParam[6] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[7] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[8] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[9] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[10] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

        
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastResCenter_ups", dbParam);
            return Convert.ToInt32(dbParam[10].Value);
        }


        public int delete(string ResCenId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@ResCenId", DbParameter.DbType.Int, 1, Convert.ToInt32(ResCenId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastResCenter_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
