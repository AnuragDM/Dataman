using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BAL.TransBeatPlan
{
    public class BeatPlan
    {
        public int Insert(string docID, int userID, DateTime planneddate, int cityId, int areaId, int beatId, string appStatus, int appBy, string appRemark, DateTime startdate,string smID)
        {

            DbParameter[] dbParam = new DbParameter[14];
            dbParam[0] = new DbParameter("@BeatPlanId", DbParameter.DbType.BigInt, 1, 0);
            dbParam[1] = new DbParameter("@DocId", DbParameter.DbType.VarChar, 50, docID);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@PlannedDate ", DbParameter.DbType.DateTime, 8, planneddate);
            dbParam[4] = new DbParameter("@CityId", DbParameter.DbType.Int, 1, cityId);
            dbParam[5] = new DbParameter("@AreaId", DbParameter.DbType.Int, 1, areaId);
            dbParam[6] = new DbParameter("@BeatId", DbParameter.DbType.Int, 1, beatId);
            dbParam[7] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 15, appStatus);
            dbParam[8] = new DbParameter("@AppBy", DbParameter.DbType.Int, 1, appBy);
            dbParam[9] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 8000, appRemark);
            dbParam[10] = new DbParameter("@StartDate ", DbParameter.DbType.DateTime, 8, startdate);
            dbParam[11] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            if (smID!="")
            {
                dbParam[12] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, Convert.ToInt32(smID));
            }
            else
            {
                dbParam[12] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, DBNull.Value);
            }
            dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransBeatPlan_ups", dbParam);
            return Convert.ToInt32(dbParam[13].Value);
        }

        public string GetDociId(DateTime newDate)
        {
            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, "BPLAN");
            dbParam[1] = new DbParameter("@V_Date", DbParameter.DbType.DateTime, 8, newDate);
            dbParam[2] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, ParameterDirection.Output);


            //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Insert into testtest (smid,rem) values (99,'" + dbParam[0] + "')");
            //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Insert into testtest (smid,rem) values (99,'" + dbParam[1] + "')");
            //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Insert into testtest (smid,rem) values (99,'" + dbParam[2] + "')");



            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Getdocid", dbParam);
            return Convert.ToString(dbParam[2].Value);
        }

        public string SetDociId(string mDocId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, "BPLAN");
            dbParam[1] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, mDocId);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Setdocid", dbParam);
            return Convert.ToString(1);
        }

        public int InsertTransNotification(string pro_id, int userID, DateTime Vdate, string msgUrl, string displayTitle, int Status, int fromUserId, int smId, int toSMId)
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
            dbParam[9] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smId);
            dbParam[10] = new DbParameter("@ToSMId", DbParameter.DbType.Int, 1, toSMId);
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransNotification_ups", dbParam);
            return Convert.ToInt32(dbParam[11].Value);
        }
    }
}
