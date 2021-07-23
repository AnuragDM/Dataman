using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.UserMaster
{
   public class userall
    {
       public int Insert(string UserName, string Password, string email, int Roleid, bool isAdmin, string DeptId, string DesgId, string empName, decimal CostCentre,string empSyncId)
       {
           DbParameter[] dbParam = new DbParameter[13];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.BigInt, 1, 0);
           dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 100, UserName);
           dbParam[2] = new DbParameter("@Pwd", DbParameter.DbType.VarChar, 20, Password);
           dbParam[3] = new DbParameter("@Active ", DbParameter.DbType.Bit, 1, isAdmin);
           dbParam[4] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, email);
           dbParam[5] = new DbParameter("@RoleId", DbParameter.DbType.Int, 4, Roleid);
           dbParam[6] = new DbParameter("@DeptId", DbParameter.DbType.Int, 4, DeptId);
           dbParam[7] = new DbParameter("@DesigId", DbParameter.DbType.Int, 4, DesgId);
           dbParam[8] = new DbParameter("@EmpName", DbParameter.DbType.VarChar, 50, empName);
           dbParam[9] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
           dbParam[10] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           dbParam[11] = new DbParameter("@CostCentre", DbParameter.DbType.Decimal, 15, CostCentre);
           dbParam[12] = new DbParameter("@EmpSyncId", DbParameter.DbType.VarChar, 30, empSyncId);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastLogin_ups", dbParam);
           return Convert.ToInt32(dbParam[10].Value);
       }

       public int Update(int id, string UserName, string Password, string email, int Roleid, bool isAdmin, string DeptId, string DesgId, string empName, decimal CostCentre, string empSyncId)
       {
           DbParameter[] dbParam = new DbParameter[13];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.BigInt, 1, id);
           dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 100, UserName);
           dbParam[2] = new DbParameter("@Pwd", DbParameter.DbType.VarChar, 20, Password);
           dbParam[3] = new DbParameter("@Active ", DbParameter.DbType.Bit, 1, isAdmin);
           dbParam[4] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, email);
           dbParam[5] = new DbParameter("@RoleId", DbParameter.DbType.Int, 4, Roleid);
           dbParam[6] = new DbParameter("@DeptId", DbParameter.DbType.Int, 4, DeptId);
           dbParam[7] = new DbParameter("@DesigId", DbParameter.DbType.Int, 4, DesgId);
           dbParam[8] = new DbParameter("@EmpName", DbParameter.DbType.VarChar, 50, empName);
           dbParam[9] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
           dbParam[10] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           dbParam[11] = new DbParameter("@CostCentre", DbParameter.DbType.Decimal, 15, CostCentre);
           dbParam[12] = new DbParameter("@EmpSyncId", DbParameter.DbType.VarChar, 30, empSyncId);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastLogin_ups", dbParam);
           return Convert.ToInt32(dbParam[10].Value);
       }

       public int delete(string Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastLogin_del", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }

    }
}
