using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;
namespace BAL.SalesTarget
{
   public class SalesTargetBAL
    {
       public int Insert(string TSalesTargetFromHODocId, int  PartyTypeId, decimal JanValue, decimal FebValue, decimal MarValue, decimal AprValue, decimal MayValue, decimal JunValue, decimal JulValue, decimal AugValue, decimal SepValue, decimal OctValue, decimal NovValue, decimal DecValue, int SMID, decimal Sno, int  AssignedByID, string SalesYear)
       {
           DbParameter[] dbParam = new DbParameter[20];
           dbParam[0] = new DbParameter("@TSalesTargetFromHODocId", DbParameter.DbType.VarChar, 30, TSalesTargetFromHODocId);
           dbParam[1] = new DbParameter("@PartyTypeId", DbParameter.DbType.Int, 4, PartyTypeId);
           dbParam[2] = new DbParameter("@JanValue", DbParameter.DbType.Decimal, 4, JanValue);
           dbParam[3] = new DbParameter("@FebValue", DbParameter.DbType.Decimal, 4, FebValue);
           dbParam[4] = new DbParameter("@MarValue", DbParameter.DbType.Decimal, 4, MarValue);
           dbParam[5] = new DbParameter("@AprValue", DbParameter.DbType.Decimal, 4, AprValue);
           dbParam[6] = new DbParameter("@MayValue", DbParameter.DbType.Decimal, 4, MayValue);
           dbParam[7] = new DbParameter("@JunValue", DbParameter.DbType.Decimal, 4, JunValue);
           dbParam[8] = new DbParameter("@JulValue", DbParameter.DbType.Decimal, 4, JulValue);
           dbParam[9] = new DbParameter("@AugValue", DbParameter.DbType.Decimal, 4, AugValue);
           dbParam[10] = new DbParameter("@SepValue", DbParameter.DbType.Decimal, 4, SepValue);
           dbParam[11] = new DbParameter("@OctValue", DbParameter.DbType.Decimal, 4, OctValue);
           dbParam[12] = new DbParameter("@NovValue", DbParameter.DbType.Decimal, 4, NovValue);
           dbParam[13] = new DbParameter("@DecValue", DbParameter.DbType.Decimal, 4, DecValue);
           dbParam[14] = new DbParameter("@SMID", DbParameter.DbType.Int, 4, SMID);
           dbParam[15] = new DbParameter("@Sno", DbParameter.DbType.Int, 4, Sno);
           dbParam[16] = new DbParameter("@AssignedByID", DbParameter.DbType.Int, 4, AssignedByID);
           dbParam[17] = new DbParameter("@SalesYear", DbParameter.DbType.VarChar, 255, SalesYear);
           dbParam[18] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
           dbParam[19] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_SalesTragetFromHO_ups", dbParam);
           return Convert.ToInt32(dbParam[19].Value);
       }

    }
}
