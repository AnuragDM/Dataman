using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.MastRole
{
    public class mastroleAll
    {
        public int Insert(string roleName,bool chkIsActive,string roletype)
        {
            DbParameter[] dbParam = new DbParameter[6];
            dbParam[0] = new DbParameter("@RoleId", DbParameter.DbType.BigInt, 1, 0);
            dbParam[1] = new DbParameter("@RoleName", DbParameter.DbType.VarChar, 50, roleName);
            dbParam[2] = new DbParameter("@isAdmin", DbParameter.DbType.Bit, 1, chkIsActive);
            dbParam[3] = new DbParameter("@RoleType ", DbParameter.DbType.VarChar, 25, roletype);
            dbParam[4] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[5] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastRole_ups", dbParam);
            return Convert.ToInt32(dbParam[5].Value);
        }
        public int delete(string RoleId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@RoleId", DbParameter.DbType.Int, 1, Convert.ToInt32(RoleId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastRole_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
