using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BAL.ComplaintNature
{
    public class ComplaintNature
    {
        public int Insert(string CompName,int deptId,string natureType, string EmailTo, string EmailCC,string SyncId, bool Active, DateTime CreatedDate)
        {
            DbParameter[] dbParam = new DbParameter[11];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, 0);
            dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 250, CompName);
            dbParam[2] = new DbParameter("@EmailTo", DbParameter.DbType.VarChar, 250, EmailTo);
            dbParam[3] = new DbParameter("@EmailCC", DbParameter.DbType.VarChar, 250, EmailCC);           
            dbParam[4] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[5] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[6] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[7] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            //Added By - Abhishek 05-12-2015 UAT
            dbParam[8] = new DbParameter("@DeptId", DbParameter.DbType.Int, 1, deptId);
            dbParam[9] = new DbParameter("@NatureType", DbParameter.DbType.VarChar, 15, natureType);
            //End
            dbParam[10] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastCompNat_ups", dbParam);
            return Convert.ToInt32(dbParam[10].Value);
        }

        public int Update(string Id, string CompName, int deptId, string natureType, string EmailTo, string EmailCC, string SyncId, bool Active, DateTime CreatedDate)
        {
            DbParameter[] dbParam = new DbParameter[11];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, Id);
            dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 250, CompName);
            dbParam[2] = new DbParameter("@EmailTo", DbParameter.DbType.VarChar, 250, EmailTo);
            dbParam[3] = new DbParameter("@EmailCC", DbParameter.DbType.VarChar, 250, EmailCC);
            dbParam[4] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 50, SyncId);
            dbParam[5] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
            dbParam[6] = new DbParameter("@CreatedDate", DbParameter.DbType.DateTime, 8, CreatedDate);
            dbParam[7] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            //Added By - Abhishek 05-12-2015 UAT
            dbParam[8] = new DbParameter("@DeptId", DbParameter.DbType.Int, 1, deptId);
            dbParam[9] = new DbParameter("@NatureType", DbParameter.DbType.VarChar, 15, natureType);
            //End
            dbParam[10] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastCompNat_ups", dbParam);
            return Convert.ToInt32(dbParam[10].Value);
        }


        public int delete(string Id)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, Convert.ToInt32(Id));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastCompNat_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
