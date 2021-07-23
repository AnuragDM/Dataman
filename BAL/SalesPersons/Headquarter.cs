using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.SalesPersons
{
    public class Headquarter
    {

        public int InsertUpdateBuilding(Int32 Id, string Name,string Address, int city_Id, int CreatedBy, int country_Id, int state_Id, int pincode)
        {
            DbParameter[] dbParam = new DbParameter[9];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Id);
            dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 100, Name);
          
         
            dbParam[2] = new DbParameter("@Address", DbParameter.DbType.VarChar, 150, Address);
            if (city_Id == 0)
                dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, DBNull.Value);
            else
                dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, city_Id);
            dbParam[4] = new DbParameter("@CreatedBy", DbParameter.DbType.Int, 10, CreatedBy);
            dbParam[5] = new DbParameter("@CountryId", DbParameter.DbType.Int, 10, country_Id);
            dbParam[6] = new DbParameter("@StateId", DbParameter.DbType.Int, 10, state_Id);
            dbParam[7] = new DbParameter("@Pincode", DbParameter.DbType.Int, 10, pincode);
         
            dbParam[8] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsUpdHeadquarter", dbParam);
            return Convert.ToInt32(dbParam[8].Value);
        }
        public int delete(string Id)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteHeadquarter", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
