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
   public class MaterialGroup
    {
       public int InsertMatGrp(string Name, string SyncId,string Active,string ItemCode)
       {
           DbParameter[] dbParam = new DbParameter[5];
           dbParam[0] = new DbParameter("@Name", DbParameter.DbType.VarChar, 50, Name);
           dbParam[1] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 20, SyncId);
           dbParam[2] = new DbParameter("@Active", DbParameter.DbType.VarChar, 1, Active);
           dbParam[3] = new DbParameter("@ItemCode", DbParameter.DbType.VarChar, 50, ItemCode);
           dbParam[4] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertMaterialGroupForm", dbParam);
           return Convert.ToInt32(dbParam[4].Value);
       }
       public int UpdateMatGrp(Int32 Id, string Name, string SyncId, string Active,string ItemCode)
       {
           DbParameter[] dbParam = new DbParameter[6];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Id);
           dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 50, Name);
           dbParam[2] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 20, SyncId);
           dbParam[3] = new DbParameter("@Active", DbParameter.DbType.VarChar, 1, Active);
           dbParam[4] = new DbParameter("@ItemCode", DbParameter.DbType.VarChar, 50, ItemCode);
           dbParam[5] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdateMaterialGroupForm", dbParam);
           return Convert.ToInt32(dbParam[5].Value);
       }
       public int delete(string Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@ItemId", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteMaterialGrp", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
    }
}
