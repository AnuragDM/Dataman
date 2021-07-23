using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.TransTourPlan
{
    public class TourAll
    {
        public int Insert(Int64 tourplanHId, string docID, DateTime Vdate, int userID, string distId, string purpose, string distStaff, string accDist, int cityId, string appStatus, int appBy, string appRemark, int smID, string fRemarks, string mCityId, string cityName, string mDistId, string distName, string mpurpose, string puproseName)
        {
            DbParameter[] dbParam = new DbParameter[23];
            dbParam[0] = new DbParameter("@TourPlanId", DbParameter.DbType.BigInt, 1, 0);
            dbParam[1] = new DbParameter("@DocId", DbParameter.DbType.VarChar, 50, docID);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@VDate ", DbParameter.DbType.DateTime, 8, Vdate);
            if (distId != "")
            {
                dbParam[4] = new DbParameter("@DistId", DbParameter.DbType.Int, 1, distId);
            }
            else
            {
                dbParam[4] = new DbParameter("@DistId", DbParameter.DbType.Int, 1, DBNull.Value);
            }
            //if (distId != "")
            //{
            //    dbParam[4] = new DbParameter("@DistId", DbParameter.DbType.VarChar, 8000, distId);
            //}
            //else
            //{
            //    dbParam[4] = new DbParameter("@DistId", DbParameter.DbType.VarChar, 1, DBNull.Value);
            //}
            dbParam[5] = new DbParameter("@Purpose", DbParameter.DbType.VarChar, 8000, purpose);
            dbParam[6] = new DbParameter("@AccDistributor", DbParameter.DbType.VarChar, 8000, accDist);
            dbParam[7] = new DbParameter("@CityId", DbParameter.DbType.VarChar, 8000, cityId);
            dbParam[8] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 15, appStatus);
            dbParam[9] = new DbParameter("@AppBy", DbParameter.DbType.Int, 1, appBy);
            dbParam[10] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 8000, appRemark);
            dbParam[11] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[12] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            dbParam[13] = new DbParameter("@DistStaff", DbParameter.DbType.VarChar, 8000, distStaff);
            dbParam[14] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[15] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 8000, fRemarks);
            dbParam[16] = new DbParameter("@mCityId", DbParameter.DbType.VarChar, 8000, mCityId);
            dbParam[17] = new DbParameter("@CityName", DbParameter.DbType.VarChar, 8000, cityName);
            if (distId != "")
            {
                dbParam[18] = new DbParameter("@mDistId", DbParameter.DbType.VarChar, 8000, mDistId);
            }
            else
            {
                dbParam[18] = new DbParameter("@mDistId", DbParameter.DbType.VarChar, 1, DBNull.Value);
            }
            dbParam[19] = new DbParameter("@DistName", DbParameter.DbType.VarChar, 8000, distName);
            dbParam[20] = new DbParameter("@mPurpose", DbParameter.DbType.VarChar, 8000, mpurpose);
            dbParam[21] = new DbParameter("@PuporseName", DbParameter.DbType.VarChar, 8000, puproseName);
            dbParam[22] = new DbParameter("@TourPlanHId", DbParameter.DbType.BigInt,1, tourplanHId);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransTourPlan_ups", dbParam);
            return Convert.ToInt32(dbParam[14].Value);
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

        public int Update(Int64 TourPlanId, DateTime VDate, int userID, string DistId, string Purpose, string distStaff, string AccDistributor, string CityId, int smID,string remarks,string mCityId, string cityName, string mDistId, string distName, string mpurpose, string puproseName)
        {
            DbParameter[] dbParam = new DbParameter[23];
            dbParam[0] = new DbParameter("@TourPlanId", DbParameter.DbType.BigInt, 1, TourPlanId);
            dbParam[1] = new DbParameter("@DocId", DbParameter.DbType.VarChar, 50, DBNull.Value);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@VDate ", DbParameter.DbType.DateTime, 8, VDate);
            if (DistId != "")
            {
                dbParam[4] = new DbParameter("@DistId", DbParameter.DbType.VarChar, 8000, DistId);
            }
            else
            {
                dbParam[4] = new DbParameter("@DistId", DbParameter.DbType.VarChar, 8000, DBNull.Value);
            }
            dbParam[5] = new DbParameter("@Purpose", DbParameter.DbType.VarChar, 8000, Purpose);
            dbParam[6] = new DbParameter("@AccDistributor", DbParameter.DbType.VarChar, 8000, AccDistributor);
            dbParam[7] = new DbParameter("@CityId", DbParameter.DbType.VarChar, 8000, CityId);
            dbParam[8] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 15, DBNull.Value);
            dbParam[9] = new DbParameter("@AppBy", DbParameter.DbType.Int, 1, DBNull.Value);
            dbParam[10] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 8000, DBNull.Value);
            dbParam[11] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[12] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            dbParam[13] = new DbParameter("@DistStaff", DbParameter.DbType.VarChar, 8000, distStaff);
            //dbParam[14] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[14] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 8000, remarks);
            dbParam[15] = new DbParameter("@mCityId", DbParameter.DbType.VarChar, 8000, mCityId);
            dbParam[16] = new DbParameter("@CityName", DbParameter.DbType.VarChar, 8000, cityName);
            if (DistId != "")
            {
                dbParam[17] = new DbParameter("@mDistId", DbParameter.DbType.VarChar, 8000, mDistId);
            }
            else
            {
                dbParam[17] = new DbParameter("@mDistId", DbParameter.DbType.VarChar, 1, DBNull.Value);
            }
            dbParam[18] = new DbParameter("@DistName", DbParameter.DbType.VarChar, 8000, distName);
            dbParam[19] = new DbParameter("@mPurpose", DbParameter.DbType.VarChar, 8000, mpurpose);
            dbParam[20] = new DbParameter("@PuporseName", DbParameter.DbType.VarChar, 8000, puproseName);
            dbParam[21] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[22] = new DbParameter("@TourPlanHId", DbParameter.DbType.BigInt, 1, 0);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransTourPlan_ups", dbParam);
            return Convert.ToInt32(dbParam[21].Value);
        }

        public string GetDociId(DateTime newDate)
        {
            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, "TOUP");
            dbParam[1] = new DbParameter("@V_Date", DbParameter.DbType.DateTime, 8, newDate);
            dbParam[2] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Getdocid", dbParam);
            return Convert.ToString(dbParam[2].Value);
        }

        public int CheckExistingLeave(DateTime fromdate, DateTime toDate, int userID)
        {
            DbParameter[] dbParam = new DbParameter[4];
            dbParam[0] = new DbParameter("@fromDate", DbParameter.DbType.DateTime, 8, fromdate);
            dbParam[1] = new DbParameter("@Todate", DbParameter.DbType.DateTime, 8, toDate);
            dbParam[2] = new DbParameter("@userid", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@Value", DbParameter.DbType.VarChar, 10, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_CheckDuplicaydate", dbParam);
            return Convert.ToInt32(dbParam[3].Value);
        }

        public string SetDociId(string mDocId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, "TOUP");
            dbParam[1] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, mDocId);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Setdocid", dbParam);
            return Convert.ToString(1);
        }

        public int delete(string docid)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@docid", DbParameter.DbType.VarChar, 50, docid);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransTourPlan_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

        //
        public int InsertTourPlanHeader(string docID, DateTime Vdate, int userID, string distId, string appStatus, int appBy, string appRemark, int smID, string appbySmId, DateTime fromdt, DateTime toDT,string finalRemarks)
        {
            DbParameter[] dbParam = new DbParameter[15];
            dbParam[0] = new DbParameter("@TourPlanHId", DbParameter.DbType.BigInt, 1, 0);
            dbParam[1] = new DbParameter("@DocId", DbParameter.DbType.VarChar, 50, docID);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@VDate ", DbParameter.DbType.DateTime, 8, Vdate);
            if (distId != "")
            {
                dbParam[4] = new DbParameter("@DistId", DbParameter.DbType.Int, 1, distId);
            }
            else
            {
                dbParam[4] = new DbParameter("@DistId", DbParameter.DbType.Int, 1, DBNull.Value);
            }
            dbParam[5] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 15, appStatus);
            dbParam[6] = new DbParameter("@AppBy", DbParameter.DbType.Int, 1, appBy);
            dbParam[7] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 8000, appRemark);
            dbParam[8] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[9] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            if (appbySmId != "")
            {
                dbParam[10] = new DbParameter("@AppBySMId", DbParameter.DbType.Int, 1, smID);
            }
            else
            {
                dbParam[10] = new DbParameter("@AppBySMId", DbParameter.DbType.Int, 1, DBNull.Value);
            }
            dbParam[11] = new DbParameter("@FromDate ", DbParameter.DbType.DateTime, 8, fromdt);
            dbParam[12] = new DbParameter("@ToDate ", DbParameter.DbType.DateTime, 8, toDT);
            dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[14] = new DbParameter("@TourPlanFRemarks", DbParameter.DbType.VarChar, 8000, finalRemarks);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransTourPlanHeader_ups", dbParam);
            return Convert.ToInt32(dbParam[13].Value);
        }
        //
    }
}
