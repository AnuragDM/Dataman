using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;

namespace BAL.Meet
{
  public   class MeetExpenseMaster
    {
        public int Insert(string Name)
        {
            DbParameter[] dbParam = new DbParameter[4];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, 0);
            dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 255, Name);
            dbParam[2] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[3] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetExpence_ups", dbParam);
            return Convert.ToInt32(dbParam[3].Value);
        }

        public int Update(int Id, string Name)
        {
            DbParameter[] dbParam = new DbParameter[4];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, Id);
            dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 255, Name);
            dbParam[2] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[3] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetExpence_ups", dbParam);
            return Convert.ToInt32(dbParam[3].Value);
        }

        public int delete(string ProspectId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, ProspectId);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetExpence_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
