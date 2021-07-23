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

        public int InsertTransNotification(string pro_id, int userID, DateTime Vdate, string msgUrl, string displayTitle, int Status, int fromUserId, int smID, int toSMId)
        {
            DbParameter[] dbParam = new DbParameter[12];
            dbParam[0] = new DbParameter("@NotiId", DbParameter.DbType.BigInt, 1, 0);
            dbParam[1] = new DbParameter("@pro_id", DbParameter.DbType.VarChar, 50, pro_id);
            dbParam[2] = new DbParameter("@userid", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@VDate ", DbParameter.DbType.DateTime, 8, Vdate);
            dbParam[4] = new DbParameter("@msgURL", DbParameter.DbType.VarChar, 500, msgUrl);
            dbParam[5] = new DbParameter("@displayTitle", DbParameter.DbType.VarChar, 100, displayTitle);
            dbParam[6] = new DbParameter("@Status", DbParameter.DbType.Bit, 1, Status);
            dbParam[7] = new DbParameter("@FromUserId", DbParameter.DbType.Int, 1, fromUserId);
            dbParam[8] = new DbParameter("@status1", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[9] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            dbParam[10] = new DbParameter("@ToSMId", DbParameter.DbType.Int, 1, toSMId);
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransNotification_ups", dbParam);
            return Convert.ToInt32(dbParam[11].Value);
        }
        public void InsertTransNotificationL3(string pro_id, int userID, DateTime Vdate, string msgUrl, string displayTitle, int Status, int fromUserId, int smID, int SMIDL3, int toSMId)
        {
            DbParameter[] dbParam = new DbParameter[13];
            dbParam[0] = new DbParameter("@NotiId", DbParameter.DbType.BigInt, 1, 0);
            dbParam[1] = new DbParameter("@pro_id", DbParameter.DbType.VarChar, 50, pro_id);
            dbParam[2] = new DbParameter("@userid", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@VDate ", DbParameter.DbType.DateTime, 8, Vdate);
            dbParam[4] = new DbParameter("@msgURL", DbParameter.DbType.VarChar, 500, msgUrl);
            dbParam[5] = new DbParameter("@displayTitle", DbParameter.DbType.VarChar, 100, displayTitle);
            dbParam[6] = new DbParameter("@Status", DbParameter.DbType.Bit, 1, Status);
            dbParam[7] = new DbParameter("@FromUserId", DbParameter.DbType.Int, 1, fromUserId);
            dbParam[8] = new DbParameter("@status1", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[9] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            dbParam[10] = new DbParameter("@ToSMId", DbParameter.DbType.Int, 1, SMIDL3);
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[12] = new DbParameter("@ToSMIdL1", DbParameter.DbType.Int, 1, toSMId);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransNotification_ups_L2", dbParam);
            //return Convert.ToInt32(dbParam[11].Value);
        }
    }
}
