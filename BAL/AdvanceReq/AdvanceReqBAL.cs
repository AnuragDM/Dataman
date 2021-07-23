using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BAL.AdvanceReq
{
    public class AdvanceReqBAL
    {
        public int InsertAdvanceReq(string FromDate, string ToDate, decimal amt, string Remarks, string StateID, DataTable dtVisitCity, string docID, string UserID, string FromTime, string ToTime)
        {
            DbParameter[] dbParam = new DbParameter[14];
            dbParam[0] = new DbParameter("@FromDate", DbParameter.DbType.DateTime, 8, FromDate);
            dbParam[1] = new DbParameter("@ToDate", DbParameter.DbType.DateTime, 8, ToDate);
            dbParam[2] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 18, amt);
            dbParam[3] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 4000, Remarks);
            dbParam[4] = new DbParameter("@CreatedOn", DbParameter.DbType.DateTime, 8, DateTime.UtcNow);
            dbParam[5] = new DbParameter("@StateID", DbParameter.DbType.Int, 5, Convert.ToInt32(StateID));
            dbParam[6] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[7] = new DbParameter("@DocId", DbParameter.DbType.VarChar, 30, docID);
            dbParam[8] = new DbParameter("@RecStatus", DbParameter.DbType.VarChar, 30, "Pending");
            dbParam[9] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 5, ParameterDirection.Output);
            dbParam[10] = new DbParameter("@ID", DbParameter.DbType.Int, 1, 0);
            dbParam[11] = new DbParameter("@UserID", DbParameter.DbType.Int, 5, Convert.ToInt32(UserID));
            dbParam[12] = new DbParameter("@FromTime", DbParameter.DbType.VarChar, 10, FromTime);
            dbParam[13] = new DbParameter("@ToTime", DbParameter.DbType.VarChar, 10, ToTime);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_AdvanceExpRequest_ups", dbParam);

            if(dtVisitCity!=null && dtVisitCity.Rows.Count>0)
            {
                DbParameter[] dbParam1 = new DbParameter[3];

                for(int i=0;i<dtVisitCity.Rows.Count;i++)
                {
                    dbParam1[0] = new DbParameter("@expadvid", DbParameter.DbType.Int, 5, Convert.ToInt32(dbParam[9].Value));
                    dbParam1[1] = new DbParameter("@visitcityid", DbParameter.DbType.Int, 5, Convert.ToInt32(dtVisitCity.Rows[i]["CityID"]));
                    dbParam1[2] = new DbParameter("@stateid", DbParameter.DbType.Int, 5, Convert.ToInt32(dtVisitCity.Rows[i]["StateID"]));

                    try
                    {
                        DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_AdvanceExpRequest1_Ins", dbParam1);
                    }
                    catch (Exception)
                    {

                        return -1;
                    }
                   
                }
            }
            return Convert.ToInt32(dbParam[9].Value);
    
        }

        public int UpdateAdvanceReq(int ID, string FromDate, string ToDate, decimal amt, string Remarks1, string StateID, DataTable dtVisitCity, string UserID, string FromTime, string ToTime)
        {
            DbParameter[] dbParam = new DbParameter[14];
            dbParam[0] = new DbParameter("@FromDate", DbParameter.DbType.DateTime, 8, FromDate);
            dbParam[1] = new DbParameter("@ToDate", DbParameter.DbType.DateTime, 8, ToDate);
            dbParam[2] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 18, amt);
            dbParam[3] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 4000, Remarks1);
            dbParam[4] = new DbParameter("@CreatedOn", DbParameter.DbType.DateTime, 8, DateTime.UtcNow);
            dbParam[5] = new DbParameter("@StateID", DbParameter.DbType.Int, 5, Convert.ToInt32(StateID));
            dbParam[6] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[7] = new DbParameter("@DocId", DbParameter.DbType.VarChar, 30, "");
            dbParam[8] = new DbParameter("@RecStatus", DbParameter.DbType.VarChar, 30, "Pending");
            dbParam[9] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 5, ParameterDirection.Output);
            dbParam[10] = new DbParameter("@ID", DbParameter.DbType.Int, 5, ID);
            dbParam[11] = new DbParameter("@UserID", DbParameter.DbType.Int, 5, Convert.ToInt32(UserID));
            dbParam[12] = new DbParameter("@FromTime", DbParameter.DbType.VarChar, 10, FromTime);
            dbParam[13] = new DbParameter("@ToTime", DbParameter.DbType.VarChar, 10, ToTime);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_AdvanceExpRequest_ups", dbParam);

            if (dtVisitCity != null && dtVisitCity.Rows.Count > 0)
            {
                DbParameter[] dbParamdel = new DbParameter[1];
                dbParamdel[0] = new DbParameter("@expadvid", DbParameter.DbType.Int, 5, ID);
                DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_AdvanceExpRequest1_del", dbParamdel);

                DbParameter[] dbParam1 = new DbParameter[3];

                for (int i = 0; i < dtVisitCity.Rows.Count; i++)
                {
                    dbParam1[0] = new DbParameter("@expadvid", DbParameter.DbType.Int, 5, Convert.ToInt32(ID));
                    dbParam1[1] = new DbParameter("@visitcityid", DbParameter.DbType.Int, 5, Convert.ToInt32(dtVisitCity.Rows[i]["CityID"]));
                    dbParam1[2] = new DbParameter("@stateid", DbParameter.DbType.Int, 5, Convert.ToInt32(dtVisitCity.Rows[i]["StateID"]));

                    try
                    {
                        DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_AdvanceExpRequest1_Ins", dbParam1);
                    }
                    catch (Exception)
                    {

                        return -1;
                    }

                }
            }
            return Convert.ToInt32(dbParam[9].Value);
    
        }

        public int DeleteExpAdv(string ID)
        {
            DbParameter[] dbParamdel = new DbParameter[1];
            dbParamdel[0] = new DbParameter("@expadvid", DbParameter.DbType.Int, 5, ID);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_AdvanceExpRequest1_del", dbParamdel);

            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@ID", DbParameter.DbType.Int, 10, ID);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteAdvReq", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

        public int UpdateAdvanceReqForAppAmt(int ID, decimal ApprovedAmt, string UserID)
        {
            DbParameter[] dbParam = new DbParameter[4];
            dbParam[0] = new DbParameter("@ID", DbParameter.DbType.Int, 10, ID);
            dbParam[1] = new DbParameter("@ApprovedAmt", DbParameter.DbType.Decimal, 18, ApprovedAmt);
            dbParam[2] = new DbParameter("@UserID", DbParameter.DbType.Int, 10, UserID);
            dbParam[3] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_ApproveAdvReq", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
