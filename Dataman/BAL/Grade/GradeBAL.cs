using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;

namespace BAL
{
   public class GradeBAL
   {
       public int InsertUpdateGrade(Int32 Id, string Name,decimal ImprestLimit,string Remarks,string SyncId,bool Active)
       {
           DbParameter[] dbParam = new DbParameter[7];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Id);
           dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 30, Name);
           dbParam[2] = new DbParameter("@ImprestLimit", DbParameter.DbType.Decimal,15, ImprestLimit);
           dbParam[3] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 500, Remarks);
           dbParam[4] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 30, SyncId);
           dbParam[5] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
           dbParam[6] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsUpdGrade", dbParam);
           return Convert.ToInt32(dbParam[6].Value);
       }
       public int delete(string Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteGrade", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
    }
}
