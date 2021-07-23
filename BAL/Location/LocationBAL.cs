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
   public class LocationBAL
   {
       public int InsertLocation(Int32 Parent, string Name, string SyncId, string Active, string Description, string AreaType, int CityType, int CityConveyanceType, string STDCode, string ISDCode, int BuisnessPlace, int Section, int CostCentre)
       {
           DbParameter[] dbParam = new DbParameter[14];
           dbParam[0] = new DbParameter("@UnderId", DbParameter.DbType.VarChar, 150, Parent);
           dbParam[1] = new DbParameter("@AreaName", DbParameter.DbType.VarChar, 150, Name);
           dbParam[2] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 20, SyncId);
           dbParam[3] = new DbParameter("@isActive", DbParameter.DbType.VarChar, 1, Active);
           dbParam[4] = new DbParameter("@AreaDesc", DbParameter.DbType.VarChar, 500, Description);
           dbParam[5] = new DbParameter("@AreaType", DbParameter.DbType.VarChar, 10, AreaType);
           dbParam[6] = new DbParameter("@CityType", DbParameter.DbType.Int, 1, CityType);
           dbParam[7] = new DbParameter("@CityConveyanceType", DbParameter.DbType.Int, 1, CityConveyanceType);
           dbParam[8] = new DbParameter("@STDCode", DbParameter.DbType.VarChar,10 , STDCode);
           dbParam[9] = new DbParameter("@ISDCode", DbParameter.DbType.VarChar, 10, ISDCode);
           dbParam[10] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           dbParam[11] = new DbParameter("@BuisnessPlace", DbParameter.DbType.Int, 1, BuisnessPlace);
           dbParam[12] = new DbParameter("@Section", DbParameter.DbType.Int, 1, Section);
           dbParam[13] = new DbParameter("@CostCentre", DbParameter.DbType.Int, 1, CostCentre);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertAreaForm", dbParam);
           return Convert.ToInt32(dbParam[10].Value);
       }
       public int UpdateLocation(Int32 Id, Int32 Parent, string Name, string SyncId, string Active, string Description, string AreaType, int CityType, int CityConveyanceType, string STDCode, string ISDCode, int BuisnessPlace, int Section, int CostCentre)
       {
           DbParameter[] dbParam = new DbParameter[15];
           dbParam[0] = new DbParameter("@AreaId", DbParameter.DbType.Int, 10, Id);
           dbParam[1] = new DbParameter("@UnderId", DbParameter.DbType.VarChar, 150, Parent);
           dbParam[2] = new DbParameter("@AreaName", DbParameter.DbType.VarChar, 150, Name);
           dbParam[3] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 20, SyncId);
           dbParam[4] = new DbParameter("@isActive", DbParameter.DbType.VarChar, 1, Active);
           dbParam[5] = new DbParameter("@AreaDesc", DbParameter.DbType.VarChar, 500, Description);
           dbParam[6] = new DbParameter("@AreaType", DbParameter.DbType.VarChar, 10, AreaType);
           dbParam[7] = new DbParameter("@CityType", DbParameter.DbType.Int, 1, CityType);
           dbParam[8] = new DbParameter("@CityConveyanceType", DbParameter.DbType.Int, 1, CityConveyanceType);
           dbParam[9] = new DbParameter("@STDCode", DbParameter.DbType.VarChar, 10, STDCode);
           dbParam[10] = new DbParameter("@ISDCode", DbParameter.DbType.VarChar, 10, ISDCode);
           dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           dbParam[12] = new DbParameter("@BuisnessPlace", DbParameter.DbType.Int, 1, BuisnessPlace);
           dbParam[13] = new DbParameter("@Section", DbParameter.DbType.Int, 1, Section);
           dbParam[14] = new DbParameter("@CostCentre", DbParameter.DbType.Int, 1, CostCentre);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdateAreaForm", dbParam);
           return Convert.ToInt32(dbParam[11].Value);
       }
       public int delete(string Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@AreaId", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteMastArea", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }

    }
}
