using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;

namespace BAL.Competitor
{
    public class CompetitorBAL
    {
        public int InsertComptitorEntry(Int64 VisId, string DocId, string VDate, int UserId, int PartyId, string Item, decimal Qty, decimal Rate, int SMID, string imgUrl, string remarks, string competitor, decimal Discount, string brandactivity, string meetactivity, string roadshow, string scheme, string OtherGInfo, string Active)
        {
            DbParameter[] dbParam = new DbParameter[22];
            dbParam[0] = new DbParameter("@ComptId", DbParameter.DbType.Int, 4, 0);
            dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
            dbParam[2] = new DbParameter("@DocId", DbParameter.DbType.VarChar, 30, DocId);
            dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
            dbParam[4] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
            dbParam[5] = new DbParameter("@SMID", DbParameter.DbType.BigInt, 8, SMID);
            if (VDate == "")
            {
                dbParam[6] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
            }
            else
            {
                dbParam[6] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
            }
            dbParam[7] = new DbParameter("@Item", DbParameter.DbType.VarChar, 100, Item);
            dbParam[8] = new DbParameter("@Rate", DbParameter.DbType.Decimal, 8, Rate);
            dbParam[9] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 150, Qty);
            dbParam[10] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 4, ParameterDirection.Output);
            dbParam[12] = new DbParameter("@ImgUrl", DbParameter.DbType.VarChar, 255, imgUrl);
            dbParam[13] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 8000, remarks);
            dbParam[14] = new DbParameter("@Competitor", DbParameter.DbType.VarChar, 100, competitor);
            dbParam[15] = new DbParameter("@Discount", DbParameter.DbType.Decimal, 150, Discount);
            dbParam[16] = new DbParameter("@brandactivity", DbParameter.DbType.VarChar, 8000, brandactivity);
            dbParam[17] = new DbParameter("@meetactivity", DbParameter.DbType.VarChar, 8000, meetactivity);
            dbParam[18] = new DbParameter("@roadshow", DbParameter.DbType.VarChar, 8000, roadshow);
            dbParam[19] = new DbParameter("@scheme", DbParameter.DbType.VarChar, 8000, scheme);
            dbParam[20] = new DbParameter("@OtherGInfo", DbParameter.DbType.VarChar, 8000, OtherGInfo);
            dbParam[21] = new DbParameter("@Active", DbParameter.DbType.VarChar, 1, Active);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransCompetitor_ups", dbParam);

            return Convert.ToInt32(dbParam[11].Value);
        }


        public int UpdateComptitorEntry(Int64 ComptId, Int64 VisId, string VDate, int UserId, int PartyId, string Item, decimal Qty, decimal Rate, int SMID, string imgUrl, string remarks, string competitor, decimal Discount, string brandactivity, string meetactivity, string roadshow, string scheme, string OtherGInfo, string Active)
        {
            DbParameter[] dbParam = new DbParameter[22];
            dbParam[0] = new DbParameter("@ComptId", DbParameter.DbType.Int, 4, ComptId);
            dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
            dbParam[2] = new DbParameter("@DocId", DbParameter.DbType.VarChar, 30, "");
            dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
            dbParam[4] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
            dbParam[5] = new DbParameter("@SMID", DbParameter.DbType.BigInt, 8, SMID);
            if (VDate == "")
            {
                dbParam[6] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
            }
            else
            {
                dbParam[6] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
            }
            dbParam[7] = new DbParameter("@Item", DbParameter.DbType.VarChar, 100, Item);
            dbParam[8] = new DbParameter("@Rate", DbParameter.DbType.Decimal, 8, Rate);
            dbParam[9] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 150, Qty);
            dbParam[10] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 4, ParameterDirection.Output);
            dbParam[12] = new DbParameter("@ImgUrl", DbParameter.DbType.VarChar, 255, imgUrl);
            dbParam[13] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 8000, remarks);
            dbParam[14] = new DbParameter("@Competitor", DbParameter.DbType.VarChar, 100, competitor);
            dbParam[15] = new DbParameter("@Discount", DbParameter.DbType.VarChar, 100, Discount);
            dbParam[16] = new DbParameter("@brandactivity", DbParameter.DbType.VarChar, 8000, brandactivity);
            dbParam[17] = new DbParameter("@meetactivity", DbParameter.DbType.VarChar, 8000, meetactivity);
            dbParam[18] = new DbParameter("@roadshow", DbParameter.DbType.VarChar, 8000, roadshow);
            dbParam[19] = new DbParameter("@scheme", DbParameter.DbType.VarChar, 8000, scheme);
            dbParam[20] = new DbParameter("@OtherGInfo", DbParameter.DbType.VarChar, 8000, OtherGInfo);
            dbParam[21] = new DbParameter("@Active", DbParameter.DbType.VarChar, 1, Active);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransCompetitor_ups", dbParam);
            return Convert.ToInt32(dbParam[11].Value);
        }

        public int delete(string ComptId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@ComptId", DbParameter.DbType.Int, 1, Convert.ToInt32(ComptId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransCompetitor_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
