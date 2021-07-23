using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;
using System;


namespace BAL
{
   public class ExpensesGroupBAL
   {
       public int InsertUpdateExpenseGroup(Int32 ExpGrpId, string Name, string Remarks, DateTime DateFrom, DateTime DateTo, int UserId, int SMId,string VoucherDocId,Int32 VoucherNo)
       {
           DbParameter[] dbParam = new DbParameter[10];
           dbParam[0] = new DbParameter("@ExpGrpId", DbParameter.DbType.Int, 10, ExpGrpId);
           dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 50, Name);
           dbParam[2] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 200, Remarks);
           dbParam[3] = new DbParameter("@DateFrom", DbParameter.DbType.DateTime, 15, DateFrom);
           dbParam[4] = new DbParameter("@DateTo", DbParameter.DbType.DateTime, 15, DateTo);
           dbParam[5] = new DbParameter("@UserId", DbParameter.DbType.Int, 10, UserId);
           dbParam[6] = new DbParameter("@SMId", DbParameter.DbType.Int, 10, SMId);
           dbParam[7] = new DbParameter("@VoucherDocId", DbParameter.DbType.VarChar, 40, VoucherDocId);
           dbParam[8] = new DbParameter("@VoucherNo", DbParameter.DbType.Int, 10, VoucherNo);
           dbParam[9] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsUpdExpenseGroup", dbParam);
           return Convert.ToInt32(dbParam[9].Value);
       }
       public int delete(string Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@ExpGrpId", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteExpenseGroup", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
    }
}
