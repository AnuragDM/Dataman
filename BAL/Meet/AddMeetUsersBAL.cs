using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;

namespace BAL.Meet
{
  public class AddMeetUsersBAL
    {
      public int InsertMeetParty(int MeetId, int AreaId, int BeatId, string Address, string ContactPersonName, string MobileNo, string EmailId, string Name, decimal Potential,string DOB, int PartyType)
        {
            DbParameter[] dbParam = new DbParameter[14];

            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, 0);
            dbParam[1] = new DbParameter("@MeetId", DbParameter.DbType.Int, 4, MeetId);
            dbParam[2] = new DbParameter("@AreaId", DbParameter.DbType.Int, 4, AreaId);
            dbParam[3] = new DbParameter("@BeatId", DbParameter.DbType.Int, 4, BeatId);
            dbParam[4] = new DbParameter("@Address", DbParameter.DbType.VarChar, 500, Address);
            dbParam[5] = new DbParameter("@ContactPersonName", DbParameter.DbType.VarChar, 50, ContactPersonName);
            dbParam[6] = new DbParameter("@MobileNo", DbParameter.DbType.VarChar, 10, MobileNo);
            dbParam[7] = new DbParameter("@EmailId", DbParameter.DbType.VarChar, 50, EmailId);
            dbParam[8] = new DbParameter("@Name", DbParameter.DbType.VarChar, 50,Name);
            dbParam[9] = new DbParameter("@Potential", DbParameter.DbType.Decimal,8, Potential);
            dbParam[10] = new DbParameter("@DOB", DbParameter.DbType.VarChar,30, DOB);
            dbParam[11] = new DbParameter("@PartyType", DbParameter.DbType.Int, 4, PartyType);
            dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransAddMeetUser_ups", dbParam);
            return Convert.ToInt32(dbParam[13].Value);
            
        }

        public int UpdateMeetParty(int Id, int MeetId, int AreaId, int BeatId, string Address, string ContactPersonName, string MobileNo, string EmailId, string Name, decimal Potential,DateTime DOB, int PartyType)
        {
            DbParameter[] dbParam = new DbParameter[14];
            //dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, 0);
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, Id);
            dbParam[1] = new DbParameter("@MeetId", DbParameter.DbType.Int, 4, MeetId);
            dbParam[2] = new DbParameter("@AreaId", DbParameter.DbType.Int, 4, AreaId);
            dbParam[3] = new DbParameter("@BeatId", DbParameter.DbType.Int, 4, BeatId);
            dbParam[4] = new DbParameter("@Address", DbParameter.DbType.VarChar, 500, Address);
            dbParam[5] = new DbParameter("@ContactPersonName", DbParameter.DbType.VarChar, 50, ContactPersonName);
            dbParam[6] = new DbParameter("@MobileNo", DbParameter.DbType.VarChar, 10, MobileNo);
            dbParam[7] = new DbParameter("@EmailId", DbParameter.DbType.VarChar, 50, EmailId);
            dbParam[8] = new DbParameter("@Name", DbParameter.DbType.VarChar, 50, Name);
            dbParam[9] = new DbParameter("Potential", DbParameter.DbType.Decimal,8, Potential);
            dbParam[10] = new DbParameter("@DOB", DbParameter.DbType.DateTime, 30, DOB);
            dbParam[11] = new DbParameter("PartyType", DbParameter.DbType.Int, 4, PartyType);

            

            dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "update");
            dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransAddMeetUser_ups", dbParam);
            return Convert.ToInt32(dbParam[13].Value);
        }

     

        public int delete(string ProspectId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, ProspectId);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransAddMeetUser_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

    }
}
