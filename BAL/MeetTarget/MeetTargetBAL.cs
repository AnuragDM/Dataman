using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;


namespace BAL.MeetTarget
{
   public class MeetTargetBAL
    {
        public int Insert(string TMeetTargetFromHODocId,int PartyTypeId,int JanValue,int FebValue,int MarValue,int AprValue,int MayValue,int JunValue,int JulValue,int AugValue,int SepValue,int OctValue,int NovValue,int DecValue,int SMID,int Sno,int AssignedByID,string MeetYear)
        {
            DbParameter[] dbParam = new DbParameter[20];
            dbParam[0] = new DbParameter("@TMeetTargetFromHODocId", DbParameter.DbType.VarChar,30, TMeetTargetFromHODocId);
            dbParam[1] = new DbParameter("@PartyTypeId", DbParameter.DbType.Int, 4, PartyTypeId);
            dbParam[2] = new DbParameter("@JanValue", DbParameter.DbType.Int, 4, JanValue);
            dbParam[3] = new DbParameter("@FebValue", DbParameter.DbType.Int, 4, FebValue);
            dbParam[4] = new DbParameter("@MarValue", DbParameter.DbType.Int, 4, MarValue);
            dbParam[5] = new DbParameter("@AprValue", DbParameter.DbType.Int, 4, AprValue);
            dbParam[6] = new DbParameter("@MayValue", DbParameter.DbType.Int, 4, MayValue);
            dbParam[7] = new DbParameter("@JunValue", DbParameter.DbType.Int, 4, JunValue);
            dbParam[8] = new DbParameter("@JulValue", DbParameter.DbType.Int, 4, JulValue);
            dbParam[9] = new DbParameter("@AugValue", DbParameter.DbType.Int, 4, AugValue);
            dbParam[10] = new DbParameter("@SepValue", DbParameter.DbType.Int, 4, SepValue);
            dbParam[11] = new DbParameter("@OctValue", DbParameter.DbType.Int, 4, OctValue);
            dbParam[12] = new DbParameter("@NovValue", DbParameter.DbType.Int, 4, NovValue);
            dbParam[13] = new DbParameter("@DecValue", DbParameter.DbType.Int, 4, DecValue);
            dbParam[14] = new DbParameter("@SMID", DbParameter.DbType.Int, 4, SMID);
            dbParam[15] = new DbParameter("@Sno", DbParameter.DbType.Int, 4, Sno);
            dbParam[16] = new DbParameter("@AssignedByID", DbParameter.DbType.Int, 4, AssignedByID);
            dbParam[17] = new DbParameter("@MeetYear", DbParameter.DbType.VarChar, 255, MeetYear);
            dbParam[18] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[19] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MeetTragetFromHO_ups", dbParam);
            return Convert.ToInt32(dbParam[19].Value);
        }

    }
}
