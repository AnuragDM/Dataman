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
   public  class MeetAttendance
    {
     

       public int Insert(int Id,bool PresentStatus,string  PresentRemark )
       {
           DbParameter[] dbParam = new DbParameter[4];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, Id);
           dbParam[1] = new DbParameter("@PresentRemark", DbParameter.DbType.VarChar, 500, PresentRemark);
           dbParam[2] = new DbParameter("@PresentStatus", DbParameter.DbType.Bit, 1, PresentStatus);
           dbParam[3] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TMeetPlanPartyUpdateAtt_ups", dbParam);
           return Convert.ToInt32(dbParam[3].Value);
       }
    }
}
