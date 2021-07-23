using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.LeaveRequest
{
    public class LeaveAll
    {
        public int Insert(string docID, int userID, DateTime Vdate, Decimal noDays, DateTime fromdate, DateTime toDate, string reason, string appStatus, int appBy, string appRemark, int smID, int appBySMId, string strLF, string leaveString)
        {
            DbParameter[] dbParam = new DbParameter[17];
            dbParam[0] = new DbParameter("@LVRQId", DbParameter.DbType.BigInt, 1, 0);
            dbParam[1] = new DbParameter("@LVRDocId", DbParameter.DbType.VarChar, 30, docID);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@VDate ", DbParameter.DbType.DateTime, 8, Vdate);
            dbParam[4] = new DbParameter("@NoOfDays", DbParameter.DbType.Decimal, 20, noDays);
            dbParam[5] = new DbParameter("@FromDate", DbParameter.DbType.DateTime, 8, fromdate);
            dbParam[6] = new DbParameter("@ToDate", DbParameter.DbType.DateTime, 8, toDate);
            dbParam[7] = new DbParameter("@Reason", DbParameter.DbType.VarChar, 255, reason);
            dbParam[8] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 15, appStatus);
            dbParam[9] = new DbParameter("@AppBy", DbParameter.DbType.Int, 1, appBy);
            dbParam[10] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 8000, appRemark);
            dbParam[11] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            dbParam[12] = new DbParameter("@AppBySMId", DbParameter.DbType.Int, 1, DBNull.Value);
            dbParam[13] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[14] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            if (strLF!="")
            {
                dbParam[15] = new DbParameter("@LeaveFlag", DbParameter.DbType.VarChar, 1, strLF);
            }
            else
            {
                dbParam[15] = new DbParameter("@LeaveFlag", DbParameter.DbType.VarChar, 1, DBNull.Value);
            }
            dbParam[16] = new DbParameter("@LeaveString", DbParameter.DbType.VarChar, 600, leaveString);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransLeaveRequest_ups", dbParam);
            return Convert.ToInt32(dbParam[14].Value);
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

        public int Update(string LVRQId, string docID, int userID, DateTime Vdate, Decimal noDays, DateTime fromdate, DateTime toDate, string reason, string appStatus, int appBy, string appRemark)
        {
            DbParameter[] dbParam = new DbParameter[13];
            dbParam[0] = new DbParameter("@LVRQId", DbParameter.DbType.BigInt, 1, LVRQId);
            dbParam[1] = new DbParameter("@LVRDocId", DbParameter.DbType.VarChar, 30, DBNull.Value);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@VDate ", DbParameter.DbType.DateTime, 8, Vdate);
            dbParam[4] = new DbParameter("@NoOfDays", DbParameter.DbType.Decimal, 20, noDays);
            dbParam[5] = new DbParameter("@FromDate", DbParameter.DbType.DateTime, 8, fromdate);
            dbParam[6] = new DbParameter("@ToDate", DbParameter.DbType.DateTime, 8, toDate);
            dbParam[7] = new DbParameter("@Reason", DbParameter.DbType.VarChar, 255, reason);
            dbParam[8] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 15, appStatus);
            dbParam[9] = new DbParameter("@AppBy", DbParameter.DbType.Int, 1, appBy);
            dbParam[10] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 8000, appRemark);
            dbParam[11] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[12] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransLeaveRequest_ups", dbParam);
            return Convert.ToInt32(dbParam[12].Value);
        }

        //Added As Per UAT On Date- 08-12-2015
        public int UpdateLeaveStatusTable(string LVRQId, int userID, DateTime Vdate, Decimal noDays, DateTime fromdate, DateTime toDate, string reason, string appStatus, int appBy, string appRemark, int smID, int appBySMId, string strLF, string leaveString)
        {
            DbParameter[] dbParam = new DbParameter[17];
            dbParam[0] = new DbParameter("@LVRQId", DbParameter.DbType.BigInt, 1, LVRQId);
            dbParam[1] = new DbParameter("@LVRDocId", DbParameter.DbType.VarChar, 30, DBNull.Value);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@VDate ", DbParameter.DbType.DateTime, 8, Vdate);
            dbParam[4] = new DbParameter("@NoOfDays", DbParameter.DbType.Decimal, 20, noDays);
            dbParam[5] = new DbParameter("@FromDate", DbParameter.DbType.DateTime, 8, fromdate);
            dbParam[6] = new DbParameter("@ToDate", DbParameter.DbType.DateTime, 8, toDate);
            dbParam[7] = new DbParameter("@Reason", DbParameter.DbType.VarChar, 255, reason);
            dbParam[8] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 15, appStatus);
            dbParam[9] = new DbParameter("@AppBy", DbParameter.DbType.Int, 1, appBy);
            dbParam[10] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 8000, appRemark);
            dbParam[11] = new DbParameter("@AppBySMId", DbParameter.DbType.Int, 1, appBySMId);
            dbParam[12] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "UpdateForTable");
            dbParam[13] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            dbParam[14] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            if (strLF != "")
            {
                dbParam[15] = new DbParameter("@LeaveFlag", DbParameter.DbType.VarChar, 1, strLF);
            }
            else
            {
                dbParam[15] = new DbParameter("@LeaveFlag", DbParameter.DbType.VarChar, 1, DBNull.Value);
            }
            dbParam[16] = new DbParameter("@LeaveString", DbParameter.DbType.VarChar, 600, leaveString);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransLeaveRequest_ups", dbParam);
            return Convert.ToInt32(dbParam[14].Value);
        }
        //End

        public int UpdateLeaveStatus(Int64 LVRQId, int userID, string appStatus, int appBy, string appRemark, int smID, int appBYSMId)
        {
            DbParameter[] dbParam = new DbParameter[17];
            dbParam[0] = new DbParameter("@LVRQId", DbParameter.DbType.BigInt, 1, LVRQId);
            dbParam[1] = new DbParameter("@LVRDocId", DbParameter.DbType.VarChar, 30, DBNull.Value);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@VDate ", DbParameter.DbType.DateTime, 8, DBNull.Value);
            dbParam[4] = new DbParameter("@NoOfDays", DbParameter.DbType.Decimal, 20, DBNull.Value);
            dbParam[5] = new DbParameter("@FromDate", DbParameter.DbType.DateTime, 8, DBNull.Value);
            dbParam[6] = new DbParameter("@ToDate", DbParameter.DbType.DateTime, 8, DBNull.Value);
            dbParam[7] = new DbParameter("@Reason", DbParameter.DbType.VarChar, 255, DBNull.Value);
            dbParam[8] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 15, appStatus);
            dbParam[9] = new DbParameter("@AppBy", DbParameter.DbType.Int, 1, appBy);
            dbParam[10] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 8000, appRemark);
            dbParam[11] = new DbParameter("@AppBySMId", DbParameter.DbType.Int, 1, appBYSMId);
            dbParam[12] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[13] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            dbParam[14] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[15] = new DbParameter("@LeaveFlag", DbParameter.DbType.VarChar, 1, DBNull.Value);
            dbParam[16] = new DbParameter("@LeaveString", DbParameter.DbType.VarChar, 600, DBNull.Value);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransLeaveRequest_ups", dbParam);
            return Convert.ToInt32(dbParam[14].Value);
        }

        public string GetDociId(DateTime newDate)
        {
            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, "LVR");
            dbParam[1] = new DbParameter("@V_Date", DbParameter.DbType.DateTime, 8, newDate);
            dbParam[2] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Getdocid", dbParam);
            return Convert.ToString(dbParam[2].Value);
        }

        public int CheckExistingLeave(DateTime fromdate, DateTime toDate, int smID)
        {
            DbParameter[] dbParam = new DbParameter[4];
            dbParam[0] = new DbParameter("@fromDate", DbParameter.DbType.DateTime, 8, fromdate);
            dbParam[1] = new DbParameter("@Todate", DbParameter.DbType.DateTime, 8, toDate);
            dbParam[2] = new DbParameter("@SmId", DbParameter.DbType.Int, 1, smID);
            dbParam[3] = new DbParameter("@Value", DbParameter.DbType.VarChar, 10, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_CheckDuplicaydate", dbParam);
            return Convert.ToInt32(dbParam[3].Value);
        }

        //Added 08-12-2015
        public int CheckExistingLeaveOnUpdate(DateTime fromdate, DateTime toDate, int smID, string lvrQid)
        {
            DbParameter[] dbParam = new DbParameter[5];
            dbParam[0] = new DbParameter("@fromDate", DbParameter.DbType.DateTime, 8, fromdate);
            dbParam[1] = new DbParameter("@Todate", DbParameter.DbType.DateTime, 8, toDate);
            dbParam[2] = new DbParameter("@SmId", DbParameter.DbType.Int, 1, smID);
            dbParam[3] = new DbParameter("@Value", DbParameter.DbType.VarChar, 10, ParameterDirection.Output);
            dbParam[4] = new DbParameter("@LVRQId", DbParameter.DbType.Int, 1, Convert.ToInt32(lvrQid));
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_CheckDuplicaydateOnUpdate", dbParam);
            return Convert.ToInt32(dbParam[3].Value);
        }
        //End

        public string SetDociId(string mDocId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, "LVR");
            dbParam[1] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, mDocId);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Setdocid", dbParam);
            return Convert.ToString(1);
        }

        public int delete(string LVRQId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@LVRQId", DbParameter.DbType.Int, 1, Convert.ToInt64(LVRQId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransLeaveRequest_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

        public int CheckExistingDSR(DateTime fromdate, DateTime toDate, int smID)
        {
            DbParameter[] dbParam = new DbParameter[4];
            dbParam[0] = new DbParameter("@fromDate", DbParameter.DbType.DateTime, 8, fromdate);
            dbParam[1] = new DbParameter("@Todate", DbParameter.DbType.DateTime, 8, toDate);
            dbParam[2] = new DbParameter("@SmId", DbParameter.DbType.Int, 1, smID);
            dbParam[3] = new DbParameter("@Value", DbParameter.DbType.VarChar, 10, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CheckDSRDateForLeave", dbParam);
            return Convert.ToInt32(dbParam[3].Value);
        }

        public void InsertTransNotificationL3(string pro_id, int userID, DateTime Vdate, string msgUrl, string displayTitle, int Status, int fromUserId, int smID, int SMIDL3,int toSMId)
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
            dbParam[12] = new DbParameter("@ToSMIdL1", DbParameter.DbType.Int, 1,  toSMId);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransNotification_ups_L2", dbParam);
            //return Convert.ToInt32(dbParam[11].Value);
        }

        public int UpdateLeaveStatus1(Int64 LVRQId, int userID, string appStatus, int appBy, string appRemark, int smID, int appBYSMId,string RejectionRemark,DateTime CurrDate)
        {
            DbParameter[] dbParam = new DbParameter[18];
            dbParam[0] = new DbParameter("@LVRQId", DbParameter.DbType.BigInt, 1, LVRQId);
            dbParam[1] = new DbParameter("@LVRDocId", DbParameter.DbType.VarChar, 30, DBNull.Value);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@VDate ", DbParameter.DbType.DateTime, 8, DBNull.Value);
            dbParam[4] = new DbParameter("@NoOfDays", DbParameter.DbType.Decimal, 20, DBNull.Value);
            dbParam[5] = new DbParameter("@FromDate", DbParameter.DbType.DateTime, 8, DBNull.Value);
            dbParam[6] = new DbParameter("@ToDate", DbParameter.DbType.DateTime, 8, DBNull.Value);
            dbParam[7] = new DbParameter("@Reason", DbParameter.DbType.VarChar, 255, DBNull.Value);
            dbParam[8] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 15, appStatus);
            dbParam[9] = new DbParameter("@AppBy", DbParameter.DbType.Int, 1, appBy);
            dbParam[10] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 8000, appRemark);
            dbParam[11] = new DbParameter("@AppBySMId", DbParameter.DbType.Int, 1, appBYSMId);
            dbParam[12] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[13] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            dbParam[14] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[15] = new DbParameter("@LeaveFlag", DbParameter.DbType.VarChar, 1, DBNull.Value);
            dbParam[16] = new DbParameter("@RejRemark", DbParameter.DbType.VarChar, 4000, RejectionRemark);
            dbParam[17] = new DbParameter("@RejDate", DbParameter.DbType.DateTime, 8, CurrDate);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransLeaveRequest_ups_L3", dbParam);
            return Convert.ToInt32(dbParam[14].Value);
        }
        public int InsertTransNotificationnew(string pro_id, int userID, DateTime Vdate, string msgUrl, string displayTitle, int Status, int fromUserId, int smID, int toSMId, int Visitsmid)
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
            if (toSMId == 0)
                dbParam[10] = new DbParameter("@ToSMId", DbParameter.DbType.Int, 1, DBNull.Value);
            else
                dbParam[10] = new DbParameter("@ToSMId", DbParameter.DbType.Int, 1, toSMId);
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);


            if (Visitsmid == 0)
                dbParam[12] = new DbParameter("@Visitsmid", DbParameter.DbType.Int, 1, DBNull.Value);
            else
                dbParam[12] = new DbParameter("@Visitsmid", DbParameter.DbType.Int, 1, Visitsmid);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransNotification_new_ups", dbParam);
            return Convert.ToInt32(dbParam[11].Value);
        }

        //Added 14/10/2020 By Akanksha

        public string RangeValidate(string value, string caption, string varType = "")
        {
            string result = "0";

            switch (value.Trim())
            {
                case "":
                    result = caption + " value is not found";
                    break;
                case null:
                    result = caption + " value is not found";
                    break;
                case "0":
                    result = caption + " value Can not be zero.";
                    break;
                default:
                    switch (varType.ToUpper())
                    {
                        case "DATE":
                            DateTime n;
                            if (DateTime.TryParseExact(value, "dd/MMM/yyyy", new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out n))
                            {
                                //ok
                            }
                            else
                            {
                                result = caption + " is not valid Date Format.";
                            }
                            break;
                        case "INT":
                            int intt;
                            if (int.TryParse(value, out intt))
                            {
                                //ok
                            }
                            else
                            {
                                result = caption + " value can not be alphabates.";
                                return result;
                            }
                            break;
                        default:
                            //ok
                            break;
                    }

                    break;
            }


            return result;
        }
    }
}
