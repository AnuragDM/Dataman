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
  public  class MeetPlanEntryBAL
    {
      public int Insert(string MeetPlanDocId, int UserId, int SMId, int AreaId, int MeetTypeId, int Usertype, int IndId, int DistId, int NoOfUser, string FromTime, string ToTime, string Venue, string Comments, int typeOfGiftEnduser, decimal valueofEnduser, string typeOfGiftRetailer, decimal valueofRetailer, string MeetDate, string MeetName, string MeetLoaction, decimal LambBudget, int SeniorId, string VenueId, decimal ExpShareDist, decimal ExpShareSelf, int SchId, int NoStaff, bool PriSec, string AppStatus, int RetailerPartyID,string parrtyId)
        {
            DbParameter[] dbParam = new DbParameter[34];
            dbParam[0] = new DbParameter("@MeetPlanId", DbParameter.DbType.Int, 4, 0);
            dbParam[1] = new DbParameter("@MeetPlanDocId", DbParameter.DbType.VarChar,30, MeetPlanDocId);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int,4, UserId);
            dbParam[3] = new DbParameter("@SMId", DbParameter.DbType.Int,4, SMId);
            dbParam[4] = new DbParameter("@AreaId", DbParameter.DbType.Int, 4, AreaId);
            dbParam[5] = new DbParameter("@MeetTypeId", DbParameter.DbType.Int,4, MeetTypeId);
            dbParam[6] = new DbParameter("@Usertype", DbParameter.DbType.Int, 4, Usertype);
            dbParam[7] = new DbParameter("@IndId", DbParameter.DbType.Int, 4, IndId);
            dbParam[8] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
            dbParam[9] = new DbParameter("@NoOfUser", DbParameter.DbType.Int, 4, NoOfUser);
            dbParam[10] = new DbParameter("@FromTime", DbParameter.DbType.VarChar,10, FromTime);
            dbParam[11] = new DbParameter("@ToTime", DbParameter.DbType.VarChar, 255, ToTime);
            dbParam[12] = new DbParameter("@Venue", DbParameter.DbType.VarChar, 255, Venue);
            dbParam[13] = new DbParameter("@Comments", DbParameter.DbType.VarChar, 4000, Comments);
            dbParam[14] = new DbParameter("@typeOfGiftEnduser", DbParameter.DbType.Int, 255, typeOfGiftEnduser);
            dbParam[15] = new DbParameter("@valueofEnduser", DbParameter.DbType.Decimal,8, valueofEnduser);
            dbParam[16] = new DbParameter("@typeOfGiftRetailer", DbParameter.DbType.VarChar, 255, typeOfGiftRetailer);
            dbParam[17] = new DbParameter("@valueofRetailer", DbParameter.DbType.Decimal,8, valueofRetailer);
            dbParam[18] = new DbParameter("@MeetDate", DbParameter.DbType.DateTime,10,Convert.ToDateTime(MeetDate));
            dbParam[19] = new DbParameter("@MeetName", DbParameter.DbType.VarChar, 255, MeetName);
            dbParam[20] = new DbParameter("@MeetLoaction", DbParameter.DbType.VarChar, 255, MeetLoaction);
            dbParam[21] = new DbParameter("@LambBudget", DbParameter.DbType.Decimal,8, LambBudget);
            dbParam[22] = new DbParameter("@SeniorId", DbParameter.DbType.Int,4, SeniorId);
            dbParam[23] = new DbParameter("@VenueId", DbParameter.DbType.VarChar, 255, VenueId);
            dbParam[24] = new DbParameter("@ExpShareDist", DbParameter.DbType.Decimal, 8, ExpShareDist);
            dbParam[25] = new DbParameter("@ExpShareSelf", DbParameter.DbType.Decimal,8, ExpShareSelf);
            dbParam[26] = new DbParameter("@SchId", DbParameter.DbType.Int,4, SchId);
            dbParam[27] = new DbParameter("@NoStaff", DbParameter.DbType.Int,4, NoStaff);
            dbParam[28] = new DbParameter("@PriSec", DbParameter.DbType.Bit,1, PriSec);
            dbParam[29] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 255, AppStatus);
            dbParam[30] = new DbParameter("@RetailerPartyID", DbParameter.DbType.Int,4,RetailerPartyID);

            
            dbParam[31] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[32] = new DbParameter("@PartyId", DbParameter.DbType.VarChar, 255, parrtyId);
            dbParam[33] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransMeetPlanEntry_ups", dbParam);
            return Convert.ToInt32(dbParam[33].Value);
        }

      public int Update(Int64 MeetPlanId, string MeetPlanDocId, int UserId, int SMId, int AreaId, int MeetTypeId, int Usertype, int IndId, int DistId, int NoOfUser, string FromTime, string ToTime, string Venue, string Comments, int typeOfGiftEnduser, decimal valueofEnduser, string typeOfGiftRetailer, decimal valueofRetailer, string MeetDate, string MeetName, string MeetLoaction, decimal LambBudget, int SeniorId, string VenueId, decimal ExpShareDist, decimal ExpShareSelf, int SchId, int NoStaff, bool PriSec, string AppStatus, int RetailerPartyID,string PartyId)
        {
            DbParameter[] dbParam = new DbParameter[34];
            dbParam[0] = new DbParameter("@MeetPlanId", DbParameter.DbType.Int, 8, MeetPlanId);
            dbParam[1] = new DbParameter("@MeetPlanDocId", DbParameter.DbType.VarChar, 30, MeetPlanDocId);
            dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
            dbParam[3] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
            dbParam[4] = new DbParameter("@AreaId", DbParameter.DbType.Int, 4, AreaId);
            dbParam[5] = new DbParameter("@MeetTypeId", DbParameter.DbType.Int, 4, MeetTypeId);
            dbParam[6] = new DbParameter("@Usertype", DbParameter.DbType.Int, 4, Usertype);
            dbParam[7] = new DbParameter("@IndId", DbParameter.DbType.Int, 4, IndId);
            dbParam[8] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
            dbParam[9] = new DbParameter("@NoOfUser", DbParameter.DbType.Int, 4, NoOfUser);
            dbParam[10] = new DbParameter("@FromTime", DbParameter.DbType.VarChar, 10, FromTime);
            dbParam[11] = new DbParameter("@ToTime", DbParameter.DbType.VarChar, 255, ToTime);
            dbParam[12] = new DbParameter("@Venue", DbParameter.DbType.VarChar, 255, Venue);
            dbParam[13] = new DbParameter("@Comments", DbParameter.DbType.VarChar, 4000, Comments);
            dbParam[14] = new DbParameter("@typeOfGiftEnduser", DbParameter.DbType.Int, 255, typeOfGiftEnduser);
            dbParam[15] = new DbParameter("@valueofEnduser", DbParameter.DbType.Decimal, 8, valueofEnduser);
            dbParam[16] = new DbParameter("@typeOfGiftRetailer", DbParameter.DbType.VarChar, 255, typeOfGiftRetailer);
            dbParam[17] = new DbParameter("@valueofRetailer", DbParameter.DbType.Decimal, 8, valueofRetailer);
            dbParam[18] = new DbParameter("@MeetDate", DbParameter.DbType.DateTime, 10, (MeetDate));
            dbParam[19] = new DbParameter("@MeetName", DbParameter.DbType.VarChar, 255, MeetName);
            dbParam[20] = new DbParameter("@MeetLoaction", DbParameter.DbType.VarChar, 255, MeetLoaction);
            dbParam[21] = new DbParameter("@LambBudget", DbParameter.DbType.Decimal, 8, LambBudget);
            dbParam[22] = new DbParameter("@SeniorId", DbParameter.DbType.Int, 4, SeniorId);
            dbParam[23] = new DbParameter("@VenueId", DbParameter.DbType.VarChar, 255, VenueId);
            dbParam[24] = new DbParameter("@ExpShareDist", DbParameter.DbType.Decimal, 8, ExpShareDist);
            dbParam[25] = new DbParameter("@ExpShareSelf", DbParameter.DbType.Decimal, 8, ExpShareSelf);
            dbParam[26] = new DbParameter("@SchId", DbParameter.DbType.Int, 4, SchId);
            dbParam[27] = new DbParameter("@NoStaff", DbParameter.DbType.Int, 4, NoStaff);
            dbParam[28] = new DbParameter("@PriSec", DbParameter.DbType.Bit, 1, PriSec);
            dbParam[29] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 255, AppStatus);

            dbParam[30] = new DbParameter("@RetailerPartyID", DbParameter.DbType.Int, 4, RetailerPartyID);


            dbParam[31] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[32] = new DbParameter("@PartyId", DbParameter.DbType.VarChar, 255, PartyId);
            dbParam[33] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransMeetPlanEntry_ups", dbParam);
            return Convert.ToInt32(dbParam[33].Value);
        }

        public int delete(string ProspectId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@MeetPlanId", DbParameter.DbType.Int, 4, ProspectId);
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastMeetExpence_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

        public int InsertMeetProduct(Int64 MeetPlanId,int SNo,int ItemGrpId,int ItemId,int ClassID,int SgmentID)
        {
            DbParameter[] dbParam = new DbParameter[8];
            dbParam[0] = new DbParameter("@MeetPlanId", DbParameter.DbType.Int, 8, MeetPlanId);
            dbParam[1] = new DbParameter("@SNo", DbParameter.DbType.Int, 4, SNo);
            dbParam[2] = new DbParameter("@ItemGrpId", DbParameter.DbType.Int, 4, ItemGrpId);
            dbParam[3] = new DbParameter("@ItemId", DbParameter.DbType.Int, 4, ItemId);
            dbParam[4] = new DbParameter("@ClassID", DbParameter.DbType.Int, 4, ClassID);
            dbParam[5] = new DbParameter("@SegmentID", DbParameter.DbType.Int, 4, SgmentID);

            dbParam[6] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[7] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransMeetPlanProduct_ups", dbParam);
            return Convert.ToInt32(dbParam[7].Value);
        }

        public int InsertMeetParty(Int64 MeetPlanId, int SNo, int PartyId, string Mobile, string Address1, string PresentRemark)
        {
            DbParameter[] dbParam = new DbParameter[8];
            dbParam[0] = new DbParameter("@MeetPlanId", DbParameter.DbType.Int, 8, MeetPlanId);
            dbParam[1] = new DbParameter("@SNo", DbParameter.DbType.Int, 4, SNo);
            dbParam[2] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
            dbParam[3] = new DbParameter("@Address1", DbParameter.DbType.VarChar, 255, Address1);
            dbParam[4] = new DbParameter("@Mobile", DbParameter.DbType.VarChar, 30, Mobile);
            dbParam[5] = new DbParameter("@PresentRemark", DbParameter.DbType.VarChar,255, PresentRemark);
            dbParam[6] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[7] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransMeetPlanPartyList_ups", dbParam);
            return Convert.ToInt32(dbParam[7].Value);
        }
        public int InsertTransNotification(string pro_id, int userID, DateTime Vdate, string msgUrl, string displayTitle, int Status, int fromUserId, int smID, int toSMId)
        {
            DbParameter[] dbParam = new DbParameter[12];
            dbParam[0] = new DbParameter("@NotiId", DbParameter.DbType.BigInt, 1, 0);
            dbParam[1] = new DbParameter("@pro_id", DbParameter.DbType.VarChar, 50, pro_id);
            dbParam[2] = new DbParameter("@userid", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@VDate ", DbParameter.DbType.DateTime, 8, Vdate);
            dbParam[4] = new DbParameter("@msgURL", DbParameter.DbType.VarChar, 500, msgUrl);
            dbParam[5] = new DbParameter("@displayTitle", DbParameter.DbType.VarChar, 500, displayTitle);
            dbParam[6] = new DbParameter("@Status", DbParameter.DbType.Bit, 1, Status);
            dbParam[7] = new DbParameter("@FromUserId", DbParameter.DbType.Int, 1, fromUserId);
            dbParam[8] = new DbParameter("@status1", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[9] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            dbParam[10] = new DbParameter("@ToSMId", DbParameter.DbType.Int, 1, toSMId);
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransNotification_ups", dbParam);
            return Convert.ToInt32(dbParam[11].Value);
        }
        public void InsertTransNotificationL3(string pro_id, int userID, DateTime Vdate, string msgUrl, string displayTitle, int Status, int fromUserId, int smID, int SMIDL3, int toSMId)
        {
            DbParameter[] dbParam = new DbParameter[13];
            dbParam[0] = new DbParameter("@NotiId", DbParameter.DbType.BigInt, 1, 0);
            dbParam[1] = new DbParameter("@pro_id", DbParameter.DbType.VarChar, 50, pro_id);
            dbParam[2] = new DbParameter("@userid", DbParameter.DbType.Int, 1, userID);
            dbParam[3] = new DbParameter("@VDate ", DbParameter.DbType.DateTime, 8, Vdate);
            dbParam[4] = new DbParameter("@msgURL", DbParameter.DbType.VarChar, 500, msgUrl);
            dbParam[5] = new DbParameter("@displayTitle", DbParameter.DbType.VarChar, 500, displayTitle);
            dbParam[6] = new DbParameter("@Status", DbParameter.DbType.Bit, 1, Status);
            dbParam[7] = new DbParameter("@FromUserId", DbParameter.DbType.Int, 1, fromUserId);
            dbParam[8] = new DbParameter("@status1", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[9] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            dbParam[10] = new DbParameter("@ToSMId", DbParameter.DbType.Int, 1, SMIDL3);
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[12] = new DbParameter("@ToSMIdL1", DbParameter.DbType.Int, 1, toSMId);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransNotification_ups_L2", dbParam);
            //return Convert.ToInt32(dbParam[11].Value);
        }
     
    }
}
