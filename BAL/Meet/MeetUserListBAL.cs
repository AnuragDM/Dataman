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
  public class MeetUserListBAL
    {
      public int InsertParty(string PartyName, string Address1, string Address2, int CityId, int AreaId, int BeatId, int UnderId, string Pin, string Mobile, string Phone, string Remark, string SyncID, int IndId, decimal Potential, bool Active, int PartyType, string ContactPerson, string CSTNo, string VatTin, string ServiceTax, string PanNo, int BlockBy, string BlockDate, string BlockReason, int CreatedUserId)
      {
          DbParameter[] dbParam = new DbParameter[26];
          dbParam[0] = new DbParameter("@PartyName", DbParameter.DbType.VarChar, 150, PartyName);
          dbParam[1] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 150, Address1);
          dbParam[2] = new DbParameter("@Address2", DbParameter.DbType.VarChar, 150, Address2);
          dbParam[3] = new DbParameter("@CityId", DbParameter.DbType.Int, 4, CityId);

          dbParam[4] = new DbParameter("@AreaId", DbParameter.DbType.Int, 4, AreaId);
          dbParam[5] = new DbParameter("@BeatId", DbParameter.DbType.Int, 4, BeatId);
          dbParam[6] = new DbParameter("@UnderId", DbParameter.DbType.Int, 4, UnderId);
          dbParam[7] = new DbParameter("@SyncID", DbParameter.DbType.VarChar, 4, SyncID);

          dbParam[8] = new DbParameter("@IndId", DbParameter.DbType.Int, 4, IndId);
          dbParam[9] = new DbParameter("@Potential", DbParameter.DbType.Decimal, 8, Potential);
          dbParam[10] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
          dbParam[11] = new DbParameter("@PartyType", DbParameter.DbType.Int, 4, PartyType);
          dbParam[12] = new DbParameter("@Pin", DbParameter.DbType.VarChar, 6, Pin);
          dbParam[13] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 12, Mobile);
          dbParam[14] = new DbParameter("@Phone", DbParameter.DbType.VarChar, 150, Phone);
          dbParam[15] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 4000, Remark);
          dbParam[16] = new DbParameter("@ContactPerson", DbParameter.DbType.VarChar, 150, ContactPerson);
          dbParam[17] = new DbParameter("@CSTNo", DbParameter.DbType.VarChar, 150, CSTNo);
          dbParam[18] = new DbParameter("@VatTin", DbParameter.DbType.VarChar, 150, VatTin);
          dbParam[19] = new DbParameter("@ServiceTax", DbParameter.DbType.VarChar, 150, ServiceTax);
          dbParam[20] = new DbParameter("@PanNo", DbParameter.DbType.VarChar, 150, PanNo);
          dbParam[21] = new DbParameter("@BlockBy", DbParameter.DbType.Int, 4, BlockBy);
          if (BlockDate == "")
          {
              dbParam[22] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[22] = new DbParameter("@BlockDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(BlockDate));
          }
        
          dbParam[23] = new DbParameter("@BlockReason", DbParameter.DbType.VarChar, 150, BlockReason);
          dbParam[24] = new DbParameter("@CreatedUserId", DbParameter.DbType.Int, 4, CreatedUserId);

          dbParam[25] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertPartyForm", dbParam);
          return Convert.ToInt32(dbParam[25].Value);
      }

      public int InsertMeetParty(Int64 MeetPlanId, int SNo, int PartyId, string Mobile, string Address1, string PresentRemark)
      {
          DbParameter[] dbParam = new DbParameter[8];
          dbParam[0] = new DbParameter("@MeetPlanId", DbParameter.DbType.Int, 8, MeetPlanId);
          dbParam[1] = new DbParameter("@SNo", DbParameter.DbType.Int, 4, SNo);
          dbParam[2] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
          dbParam[3] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 255, Address1);
          dbParam[4] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
          dbParam[5] = new DbParameter("@PresentRemark", DbParameter.DbType.VarChar, 255, PresentRemark);
          dbParam[6] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
          dbParam[7] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransMeetPlanPartyList_ups", dbParam);
          return Convert.ToInt32(dbParam[7].Value);
      }

        public int delete(string ProspectId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, ProspectId);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastScheme_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

        
    }
}
