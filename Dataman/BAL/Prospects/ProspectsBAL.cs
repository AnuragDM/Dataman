using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;

namespace BAL.Prospects
{
  public  class ProspectsBAL
  {
      public int Insert(string Name,string Address1,string  Pin,string Email,string Mobile,string Remark,int UserId,int CityId,int State,int Country,string crDate)
      {
          DbParameter[] dbParam = new DbParameter[14];
          dbParam[0] = new DbParameter("@ProspectId", DbParameter.DbType.Int, 4, 0);
          dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar,150, Name);
          dbParam[2] = new DbParameter("@Address1", DbParameter.DbType.VarChar,150, Address1);
          dbParam[3] = new DbParameter("@Pin", DbParameter.DbType.VarChar,6, Pin);
          dbParam[4] = new DbParameter("@Email", DbParameter.DbType.VarChar,50, Email);
          dbParam[5] = new DbParameter("@Mobile", DbParameter.DbType.VarChar,10, Mobile);
          dbParam[6] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 4000, Remark);
          dbParam[7] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
          dbParam[8] = new DbParameter("@Created_Date", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(crDate));
          dbParam[9] = new DbParameter("@CityId", DbParameter.DbType.Int, 4, CityId);
          dbParam[10] = new DbParameter("@State", DbParameter.DbType.Int, 4, State);
          dbParam[11] = new DbParameter("@Country", DbParameter.DbType.Int, 4, Country);

          dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
          dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_Prospects_ups", dbParam);
          return Convert.ToInt32(dbParam[13].Value);
      }



      public int Update(int ProspectId, string Name, string Address1, string Pin, string Email, string Mobile, string Remark, int UserId, int CityId, int State, int Country, string crDate)
      {
          DbParameter[] dbParam = new DbParameter[14];
          dbParam[0] = new DbParameter("@ProspectId", DbParameter.DbType.Int, 4, ProspectId);
          dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 150, Name);
          dbParam[2] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
          dbParam[3] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
          dbParam[4] = new DbParameter("@Email", DbParameter.DbType.VarChar, 50, Email);
          dbParam[5] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 10, Mobile);
          dbParam[6] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 4000, Remark);
          dbParam[7] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
          dbParam[8] = new DbParameter("@Created_Date", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(crDate));
          dbParam[9] = new DbParameter("@CityId", DbParameter.DbType.Int, 4, CityId);
          dbParam[10] = new DbParameter("@State", DbParameter.DbType.Int, 4, State);
          dbParam[11] = new DbParameter("@Country", DbParameter.DbType.Int, 4, Country);

          dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
          dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_Prospects_ups", dbParam);
          return Convert.ToInt32(dbParam[13].Value);
      }

      public int delete(string ProspectId)
      {
          DbParameter[] dbParam = new DbParameter[2];
          dbParam[0] = new DbParameter("@ProspectId", DbParameter.DbType.Int, 4, Convert.ToInt32(ProspectId));
          dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_Prospects_del", dbParam);
          return Convert.ToInt32(dbParam[1].Value);
      }
    }
}
