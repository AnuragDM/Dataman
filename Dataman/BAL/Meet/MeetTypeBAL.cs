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
  public  class MeetTypeBAL
    {
      public int Insert(string Name, int userType,bool Invition)
        {
            DbParameter[] dbParam = new DbParameter[7];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, 0);
            dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 255, Name);
            dbParam[2] = new DbParameter("@userType", DbParameter.DbType.Int, 4, userType);
            dbParam[3] = new DbParameter("@Invition", DbParameter.DbType.Bit, 1, Invition);
            dbParam[4] = new DbParameter("@createdDate", DbParameter.DbType.DateTime, 1, DateTime.Now);
            dbParam[5] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[6] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetType_ups", dbParam);
            return Convert.ToInt32(dbParam[6].Value);
        }

        public int Update(int Id,string Name, int userType,bool Invition)
        {
            DbParameter[] dbParam = new DbParameter[7];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, Id);
            dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 255, Name);
            dbParam[2] = new DbParameter("@userType", DbParameter.DbType.Int, 4, userType);
            dbParam[3] = new DbParameter("@Invition", DbParameter.DbType.Bit, 1, Invition);
            dbParam[4] = new DbParameter("@createdDate", DbParameter.DbType.DateTime, 1, DateTime.Now);
            dbParam[5] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[6] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetType_ups", dbParam);
            return Convert.ToInt32(dbParam[6].Value);
        }

        public int delete(string ProspectId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, ProspectId);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetType_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }




        public int InsertCheckLIstMaster(string MeetTypeId,string  Narration)
        {
            DbParameter[] dbParam = new DbParameter[5];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, 0);
            dbParam[1] = new DbParameter("@MeetTypeId", DbParameter.DbType.Int,4, MeetTypeId);
            dbParam[2] = new DbParameter("@Narration", DbParameter.DbType.VarChar,255, Narration);
          
            dbParam[3] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[4] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetCheckList_ups", dbParam);
            return Convert.ToInt32(dbParam[4].Value);
        }

        public int UpdateCheckLIstMaster(int Id, string MeetTypeId, string Narration)
        {
            DbParameter[] dbParam = new DbParameter[5];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, Id);
            dbParam[1] = new DbParameter("@MeetTypeId", DbParameter.DbType.Int, 4, MeetTypeId);
            dbParam[2] = new DbParameter("@Narration", DbParameter.DbType.VarChar, 255, Narration);
            dbParam[3] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[4] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetCheckList_ups", dbParam);
            return Convert.ToInt32(dbParam[4].Value);
        }

        public int deleteCheckLIstMaster(string ProspectId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, ProspectId);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetCheckList_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }



        public int InsertBudgetMaster(int MeetTypeId,int AreaTypeId,int MeetExpId,decimal ApproxCost,decimal TotalQty,decimal EstimatedCost)
        {
            DbParameter[] dbParam = new DbParameter[9];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4,0);
            dbParam[1] = new DbParameter("@MeetTypeId", DbParameter.DbType.Int, 4, MeetTypeId);
            dbParam[2] = new DbParameter("@AreaTypeId", DbParameter.DbType.Int, 4, AreaTypeId);
            dbParam[3] = new DbParameter("@MeetExpId", DbParameter.DbType.Int, 4, MeetExpId);

            dbParam[4] = new DbParameter("@ApproxCost", DbParameter.DbType.Int, 4, ApproxCost);
            dbParam[5] = new DbParameter("@TotalQty", DbParameter.DbType.Int, 4, TotalQty);
            dbParam[6] = new DbParameter("@EstimatedCost", DbParameter.DbType.Int, 4, EstimatedCost);


            dbParam[7] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[8] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetExpBudget_ups", dbParam);
            return Convert.ToInt32(dbParam[8].Value);
        }

        public int UpdatBudgetMaster(int Id, int MeetTypeId, int AreaTypeId, int MeetExpId, decimal ApproxCost, decimal TotalQty, decimal EstimatedCost)
        {
            DbParameter[] dbParam = new DbParameter[9];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, Id);
            dbParam[1] = new DbParameter("@MeetTypeId", DbParameter.DbType.Int, 4, MeetTypeId);
            dbParam[2] = new DbParameter("@AreaTypeId", DbParameter.DbType.Int, 4, AreaTypeId);
            dbParam[3] = new DbParameter("@MeetExpId", DbParameter.DbType.Int, 4, MeetExpId);

            dbParam[4] = new DbParameter("@ApproxCost", DbParameter.DbType.Int, 4, ApproxCost);
            dbParam[5] = new DbParameter("@TotalQty", DbParameter.DbType.Int, 4, TotalQty);
            dbParam[6] = new DbParameter("@EstimatedCost", DbParameter.DbType.Int, 4, EstimatedCost);

         
            dbParam[7] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[8] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetExpBudget_ups", dbParam);
            return Convert.ToInt32(dbParam[8].Value);
        }

        public int deleteBudgetMaster(string ProspectId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, ProspectId);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetExpBudget_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }


    }
}
