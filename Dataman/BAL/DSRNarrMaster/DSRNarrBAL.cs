using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BAL.DSRNarrMaster
{
    public class DSRNarrBAL
    {
        public int Insert(string Name, string NarrType, string SortOrder, string SyncId, bool Active, DateTime CreatedDate)
        {
            DbParameter[] dbParam = new DbParameter[9];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, 0);
            dbParam[1] = new DbParameter("@name", DbParameter.DbType.VarChar, 4000, Name);
            dbParam[2] = new DbParameter("@narrtype", DbParameter.DbType.VarChar, 20, NarrType);
            dbParam[3] = new DbParameter("@sortorder", DbParameter.DbType.Int, 4000, Convert.ToInt32(SortOrder));
            dbParam[4] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[5] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[6] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[7] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[8] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastDSRNarration_ups", dbParam);
            return Convert.ToInt32(dbParam[8].Value);
        }

        public int Update(string Id, string Name, string NarrType, string SortOrder, string SyncId, bool Active, DateTime CreatedDate)
        {
            DbParameter[] dbParam = new DbParameter[9];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, Convert.ToInt32(Id));
            dbParam[1] = new DbParameter("@name", DbParameter.DbType.VarChar, 4000, Name);
            dbParam[2] = new DbParameter("@narrtype", DbParameter.DbType.VarChar, 20, NarrType);
            dbParam[3] = new DbParameter("@sortorder", DbParameter.DbType.Int, 4000, Convert.ToInt32(SortOrder));
            dbParam[4] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[5] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[6] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[7] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[8] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastDSRNarration_ups", dbParam);
            return Convert.ToInt32(dbParam[8].Value);
        }


        public int delete(string Id)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, Convert.ToInt32(Id));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastDSRNarr_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

     
      
    }
}
