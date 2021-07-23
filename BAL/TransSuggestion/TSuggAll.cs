using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.TransSuggestion
{
    public class TSuggAll
    {
        public int Insert(DateTime Vdate, string DocID, int UserId, string ComplNatId, string ItemId, string NewApplicationArea, string TechnicalAdvantage, string MakeProductBetter, string imgURL, int smID,int distID,string AType)
        {
            DbParameter[] dbParam = new DbParameter[17];
            dbParam[0] = new DbParameter("@SuggId", DbParameter.DbType.Int, 1, 0);
            dbParam[1] = new DbParameter("@DocId", DbParameter.DbType.VarChar, 30, DocID);
            dbParam[2] = new DbParameter("@Vdate", DbParameter.DbType.DateTime, 20, Vdate);
            dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, UserId);
            dbParam[4] = new DbParameter("@ComplNatId", DbParameter.DbType.Int, 8, Convert.ToInt32(ComplNatId));
            if (ItemId != "")
            {
                dbParam[5] = new DbParameter("@ItemId", DbParameter.DbType.Int, 8, Convert.ToInt32(ItemId));
            }
            else
            {
                dbParam[5] = new DbParameter("@ItemId", DbParameter.DbType.Int, 8, DBNull.Value);
            }
            dbParam[6] = new DbParameter("@NewApplicationArea", DbParameter.DbType.VarChar, 8000, NewApplicationArea);
            dbParam[7] = new DbParameter("@TechnicalAdvantage", DbParameter.DbType.VarChar, 8000, TechnicalAdvantage);
            dbParam[8] = new DbParameter("@MakeProductBetter", DbParameter.DbType.VarChar, 8000, MakeProductBetter);
            dbParam[9] = new DbParameter("@ImgUrl", DbParameter.DbType.VarChar, 255, imgURL);
            dbParam[10] = new DbParameter("@BatchNo", DbParameter.DbType.VarChar, 50, DBNull.Value);
            dbParam[11] = new DbParameter("@ManufactureDate", DbParameter.DbType.DateTime, 8, DBNull.Value);
            if(smID==0)
            {
                dbParam[12] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, DBNull.Value);
            }
            else
            {
                dbParam[12] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            }
           
            if (distID == 0)
            {
                dbParam[13] = new DbParameter("@DistId", DbParameter.DbType.Int, 1, DBNull.Value);
            }
            else
            {
                dbParam[13] = new DbParameter("@DistId", DbParameter.DbType.Int, 1, distID);
            }
            dbParam[14] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[15] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[16] = new DbParameter("@AType", DbParameter.DbType.VarChar,1, AType);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransSuggestion_ups", dbParam);
            return Convert.ToInt32(dbParam[15].Value);
        }

        public string GetDociId(DateTime newDate)
        {
            DbParameter[] dbParam = new DbParameter[3];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, "SUGGE");
            dbParam[1] = new DbParameter("@V_Date", DbParameter.DbType.DateTime, 8, newDate);
            dbParam[2] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Getdocid", dbParam);
            return Convert.ToString(dbParam[2].Value);
        }

        public string SetDociId(string mDocId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, "SUGGE");
            dbParam[1] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, mDocId);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Setdocid", dbParam);
            return Convert.ToString(1);
        }

        public int Update(Int64 SuggId, DateTime Vdate, int UserId, string ComplNatId, string ItemId, string NewApplicationArea, string TechnicalAdvantage, string MakeProductBetter, string imgURL, int smID, int distID, string AType)
        {
            DbParameter[] dbParam = new DbParameter[17];
            dbParam[0] = new DbParameter("@SuggId", DbParameter.DbType.BigInt, 1, SuggId);
            dbParam[1] = new DbParameter("@DocId", DbParameter.DbType.VarChar, 30,DBNull.Value);
            dbParam[2] = new DbParameter("@Vdate", DbParameter.DbType.DateTime, 20, Vdate);
            dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, UserId);
            dbParam[4] = new DbParameter("@ComplNatId", DbParameter.DbType.Int, 8, Convert.ToInt32(ComplNatId));
            if (ItemId != "")
            {
                dbParam[5] = new DbParameter("@ItemId", DbParameter.DbType.Int, 8, Convert.ToInt32(ItemId));
            }
            else
            {
                dbParam[5] = new DbParameter("@ItemId", DbParameter.DbType.Int, 8, DBNull.Value);
            }
            dbParam[6] = new DbParameter("@NewApplicationArea", DbParameter.DbType.VarChar, 8000, NewApplicationArea);
            dbParam[7] = new DbParameter("@TechnicalAdvantage", DbParameter.DbType.VarChar, 8000, TechnicalAdvantage);
            dbParam[8] = new DbParameter("@MakeProductBetter", DbParameter.DbType.VarChar, 8000, MakeProductBetter);
            dbParam[9] = new DbParameter("@ImgUrl", DbParameter.DbType.VarChar, 255, imgURL);
            dbParam[10] = new DbParameter("@BatchNo", DbParameter.DbType.VarChar, 50, DBNull.Value);
            dbParam[11] = new DbParameter("@ManufactureDate", DbParameter.DbType.DateTime, 8, DBNull.Value);
            if (smID == 0)
            {
                dbParam[12] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, DBNull.Value);
            }
            else
            {
                dbParam[12] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            }
            if (distID == 0)
            {
                dbParam[13] = new DbParameter("@DistId", DbParameter.DbType.Int, 1, DBNull.Value);
            }
            else
            {
                dbParam[13] = new DbParameter("@DistId", DbParameter.DbType.Int, 1, distID);
            }
            dbParam[14] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[15] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[16] = new DbParameter("@AType", DbParameter.DbType.VarChar, 1, AType);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransSuggestion_ups", dbParam);
            return Convert.ToInt32(dbParam[15].Value);
        }
        public int delete(string SuggId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@SuggId", DbParameter.DbType.Int, 1, Convert.ToInt32(SuggId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransSuggestion_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
