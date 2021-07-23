using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;


namespace BAL.DemoEntry
{
    public class DemoEntryBAL
    {
        public int InsertDemo(Int64 VisId, string DemoDocId, int UserId, string VDate, int SMId, int PartyId, int AreaId, string Remarks, string ProductClassId, string ProductSegmentId, string ProductMatGrp,string imgURL)
        {
            DbParameter[] dbParam = new DbParameter[15];
            dbParam[0] = new DbParameter("@DemoId", DbParameter.DbType.Int, 4, 0);
            dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
            dbParam[2] = new DbParameter("@DemoDocId", DbParameter.DbType.VarChar, 30, DemoDocId);
            dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
            dbParam[4] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
            dbParam[5] = new DbParameter("@SMId", DbParameter.DbType.BigInt, 8, SMId);
            dbParam[6] = new DbParameter("@AreaId", DbParameter.DbType.VarChar, 20, AreaId);
            if (VDate == "")
            {
                dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
            }
            else
            {
                dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
            }
            dbParam[8] = new DbParameter("@ProductSegmentId", DbParameter.DbType.Int, 4, ProductSegmentId);
            dbParam[9] = new DbParameter("@ProductMatGrp", DbParameter.DbType.Int, 4, ProductMatGrp);
            dbParam[10] = new DbParameter("@ProductClassId", DbParameter.DbType.Int, 4, ProductClassId);
            dbParam[11] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 150, Remarks);
            dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 4, ParameterDirection.Output);
            dbParam[14] = new DbParameter("@ImgUrl", DbParameter.DbType.VarChar, 255, imgURL);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransDemo_ups", dbParam);
            return Convert.ToInt32(dbParam[13].Value);
        }

        public int UpdateDemo(Int64 DemoId,Int64 VisId, int UserId, string VDate, int SMId, int PartyId, int AreaId, string Remarks, string ProductClassId, string ProductSegmentId, string ProductMatGrp,string imgUrl)
        {
            DbParameter[] dbParam = new DbParameter[15];
            dbParam[0] = new DbParameter("@DemoId", DbParameter.DbType.Int, 4, DemoId);
            dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
            dbParam[2] = new DbParameter("@DemoDocId", DbParameter.DbType.VarChar, 30, "");
            dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
            dbParam[4] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
            dbParam[5] = new DbParameter("@SMId", DbParameter.DbType.BigInt, 8, SMId);
            dbParam[6] = new DbParameter("@AreaId", DbParameter.DbType.VarChar, 20, AreaId);
            if (VDate == "")
            {
                dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
            }
            else
            {
                dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
            }
            dbParam[8] = new DbParameter("@ProductSegmentId", DbParameter.DbType.Int, 4, ProductSegmentId);
            dbParam[9] = new DbParameter("@ProductMatGrp", DbParameter.DbType.Int, 4, ProductMatGrp);
            dbParam[10] = new DbParameter("@ProductClassId", DbParameter.DbType.Int, 4, ProductClassId);
            dbParam[11] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 150, Remarks);
            dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "update");
            dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 4, ParameterDirection.Output);
            dbParam[14] = new DbParameter("@ImgUrl", DbParameter.DbType.VarChar, 255, imgUrl);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransDemo_ups", dbParam);
            return Convert.ToInt32(dbParam[13].Value);
        }

        public int delete(string DemoId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@DemoId", DbParameter.DbType.Int, 1, Convert.ToInt32(DemoId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransDemo_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}

