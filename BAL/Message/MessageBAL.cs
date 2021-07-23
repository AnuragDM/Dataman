using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;

namespace BAL.Message
{
    public class MessageBAL
    {
        public int Insert(int RoleId, string Message, int AreaID,bool active)
        {
            DbParameter[] dbParam = new DbParameter[7];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, 0);
            dbParam[1] = new DbParameter("@Message", DbParameter.DbType.VarChar, 4000, Message);
            dbParam[2] = new DbParameter("@RoleId", DbParameter.DbType.Int, 4, RoleId);
            dbParam[3] = new DbParameter("@AreaID", DbParameter.DbType.Int, 4, AreaID);

            dbParam[4] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[5] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, active);
            dbParam[6] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMessage_ups", dbParam);
            return Convert.ToInt32(dbParam[6].Value);
        }
        public int Update(int Id, int RoleId, string Message, int AreaID, bool active)
        {
            DbParameter[] dbParam = new DbParameter[7];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, Id);
            dbParam[1] = new DbParameter("@Message", DbParameter.DbType.VarChar, 4000, Message);
            dbParam[2] = new DbParameter("@RoleId", DbParameter.DbType.Int, 4, RoleId);
            dbParam[3] = new DbParameter("@AreaID", DbParameter.DbType.Int, 4, AreaID);

            dbParam[4] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[5] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, active);
            dbParam[6] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMessage_ups", dbParam);
            return Convert.ToInt32(dbParam[6].Value);
        }

        public int delete(string VisId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@id", DbParameter.DbType.Int, 1, Convert.ToInt32(VisId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMessage_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

    }
}

