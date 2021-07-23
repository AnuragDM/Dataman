using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;


namespace BAL.Distributer
{
  public  class DistributerCollection
    {
     
        public int Insert(string CollDocId,int UserId ,int DistId,int SMId,string Mode,decimal Amount,DateTime PaymentDate,string  Cheque_DDNo,string  Cheque_DD_Date,string Bank,string Branch,string Remarks,string VisId, string VDate)
        {
            DbParameter[] dbParam = new DbParameter[17];
            dbParam[0] = new DbParameter("@CollId", DbParameter.DbType.Int, 1, 0);
            dbParam[1] = new DbParameter("@CollDocId", DbParameter.DbType.VarChar, 30, CollDocId);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int,4, UserId);
            dbParam[3] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
            dbParam[4] = new DbParameter("@SMId", DbParameter.DbType.BigInt,8, SMId);
            dbParam[5] = new DbParameter("@Mode", DbParameter.DbType.VarChar, 20, Mode);
            dbParam[6] = new DbParameter("@Amount", DbParameter.DbType.Decimal,8, Amount);
            dbParam[7] = new DbParameter("@PaymentDate", DbParameter.DbType.DateTime, 50, PaymentDate);
            dbParam[8] = new DbParameter("@Cheque_DDNo", DbParameter.DbType.VarChar, 10, Cheque_DDNo);

        
           if (Cheque_DD_Date == "")
           {
               dbParam[9] = new DbParameter("@Cheque_DD_Date", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
           }
           else
           {
               dbParam[9] = new DbParameter("@Cheque_DD_Date", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(Cheque_DD_Date));
           }
            dbParam[10] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
            dbParam[11] = new DbParameter("@Bank", DbParameter.DbType.VarChar, 100, Bank);
            dbParam[12] = new DbParameter("@Branch", DbParameter.DbType.VarChar, 100, Branch);
            dbParam[13] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 150, Remarks);
            dbParam[14] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
            dbParam[15] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[16] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_DistributerCollection_ups", dbParam);
            return Convert.ToInt32(dbParam[16].Value);
        }

        public int Update(int CollId,int UserId, int DistId, int SMId, string Mode, decimal Amount, DateTime PaymentDate, string Cheque_DDNo, string Cheque_DD_Date, string Bank, string Branch, string Remarks,string VisId, string VDate)
        {
            DbParameter[] dbParam = new DbParameter[17];
            dbParam[0] = new DbParameter("@CollId", DbParameter.DbType.Int, 4, CollId);
            dbParam[1] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
            dbParam[2] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
            dbParam[3] = new DbParameter("@SMId", DbParameter.DbType.BigInt, 8, SMId);
            dbParam[4] = new DbParameter("@Mode", DbParameter.DbType.VarChar, 20, Mode);
            dbParam[5] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 8, Amount);
            dbParam[6] = new DbParameter("@PaymentDate", DbParameter.DbType.DateTime, 50, PaymentDate);
            dbParam[7] = new DbParameter("@Cheque_DDNo", DbParameter.DbType.VarChar, 10, Cheque_DDNo);
            if (Cheque_DD_Date == "")
            {
                dbParam[8] = new DbParameter("@Cheque_DD_Date", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
            }
            else
            {
                dbParam[8] = new DbParameter("@Cheque_DD_Date", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(Cheque_DD_Date));
            }
            dbParam[9] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
            dbParam[10] = new DbParameter("@Bank", DbParameter.DbType.VarChar, 100, Bank);
            dbParam[11] = new DbParameter("@Branch", DbParameter.DbType.VarChar, 100, Branch);
            dbParam[12] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 150, Remarks);
            dbParam[13] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
            dbParam[14] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");           
            dbParam[15] = new DbParameter("@CollDocId", DbParameter.DbType.VarChar, 30, "");
            dbParam[16] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_DistributerCollection_ups", dbParam);
            return Convert.ToInt32(dbParam[16].Value);
        }
        public int delete(string CollId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@CollId", DbParameter.DbType.Int, 1, Convert.ToInt32(CollId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_DistributerCollection_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
