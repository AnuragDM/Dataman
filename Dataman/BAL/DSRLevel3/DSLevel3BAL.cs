using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;

namespace BAL.DSRLevel3
{
   public class DSLevel3BAL
    {
       public int Insert(string VisId, int Sno, string VDate, int UserId, int SMId, int CityId, int DistId, int ResCenId, string Colleague, string DistributorRep, int MDSRNarrId, string Remarks, string DSRType, string FromTime, string ToTime)
       {
           DbParameter[] dbParam = new DbParameter[18];
           dbParam[0] = new DbParameter("@DSRL3Id", DbParameter.DbType.Int, 4, 0);
           dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.VarChar, 30, VisId);
           dbParam[2] = new DbParameter("@Sno", DbParameter.DbType.Int, 4, Sno);
           dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
           dbParam[4] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
           dbParam[5] = new DbParameter("@CityId", DbParameter.DbType.Int, 4, CityId);
           dbParam[6] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
           dbParam[7] = new DbParameter("@ResCenId", DbParameter.DbType.Int, 4, ResCenId);
           dbParam[8] = new DbParameter("@Colleague", DbParameter.DbType.VarChar, 100, Colleague);
           dbParam[9] = new DbParameter("@DistributorRep", DbParameter.DbType.VarChar, 100, DistributorRep);
           dbParam[10] = new DbParameter("@MDSRNarrId", DbParameter.DbType.Int, 4, MDSRNarrId);
           dbParam[11] = new DbParameter("@DSRType", DbParameter.DbType.VarChar, 100, DSRType);
        
           if (VDate == "")
           {
               dbParam[12] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
           }
           else
           {
               dbParam[12] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
           }
           dbParam[13] = new DbParameter("@FromTime", DbParameter.DbType.VarChar, 10, FromTime);
           dbParam[14] = new DbParameter("@ToTime", DbParameter.DbType.VarChar, 10, ToTime);
           dbParam[15] = new DbParameter("@Remarks", DbParameter.DbType.VarChar,4000, Remarks);
           dbParam[16] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
           dbParam[17] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransDSRL3_ups", dbParam);
           return Convert.ToInt32(dbParam[17].Value);
       }



       public int Update(Int64 DSRL3Id, string VisId, int Sno, string VDate, int UserId, int SMId, int CityId, int DistId, int ResCenId, string Colleague, string DistributorRep, int MDSRNarrId, string Remarks, string DSRType, string FromTime, string ToTime)
        {
            DbParameter[] dbParam = new DbParameter[18];
            dbParam[0] = new DbParameter("@DSRL3Id", DbParameter.DbType.Int, 4, DSRL3Id);
            dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.VarChar, 30, VisId);
            dbParam[2] = new DbParameter("@Sno", DbParameter.DbType.Int, 4, Sno);
            dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
            dbParam[4] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
            dbParam[5] = new DbParameter("@CityId", DbParameter.DbType.Int, 4, CityId);
            dbParam[6] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
            dbParam[7] = new DbParameter("@ResCenId", DbParameter.DbType.Int, 4, ResCenId);
            dbParam[8] = new DbParameter("@Colleague", DbParameter.DbType.VarChar, 100, Colleague);
            dbParam[9] = new DbParameter("@DistributorRep", DbParameter.DbType.VarChar, 100, DistributorRep);
            dbParam[10] = new DbParameter("@MDSRNarrId", DbParameter.DbType.Int, 4, MDSRNarrId);
            dbParam[11] = new DbParameter("@DSRType", DbParameter.DbType.VarChar, 100, DSRType);
          
            if (VDate == "")
            {
                dbParam[12] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
            }
            else
            {
                dbParam[12] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
            }
            dbParam[13] = new DbParameter("@FromTime", DbParameter.DbType.VarChar, 10, FromTime);
            dbParam[14] = new DbParameter("@ToTime", DbParameter.DbType.VarChar, 10, ToTime);
            dbParam[15] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 4000, Remarks);
            dbParam[16] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[17] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransDSRL3_ups", dbParam);
            return Convert.ToInt32(dbParam[17].Value);
        }

        public int delete(string DSRL3Id)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@DSRL3Id", DbParameter.DbType.Int, 1, Convert.ToInt32(DSRL3Id));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransDSRL3_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
