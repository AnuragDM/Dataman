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
  public  class DSRLevel1BAL
    {

      public int Insert(string VisitDocId, int UserId, string VDate, string NextVisitDate, string Remark, int SMId, int CityId, int DistId, int nCityId, string frTime1, string frTime2, string toTime1, string toTime2, int WithUserId, string ModeOfTransport, string VehicleUsed, string Industry, bool Lock, int nWithUserId)
      {
          DbParameter[] dbParam = new DbParameter[25];
          dbParam[0] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, 0);
          dbParam[1] = new DbParameter("@VisitDocId", DbParameter.DbType.VarChar, 30, VisitDocId);
          dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
          dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          if (NextVisitDate == "")
          {
              dbParam[4] = new DbParameter("@NextVisitDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[4] = new DbParameter("@NextVisitDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(NextVisitDate));
          }
          dbParam[5] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 4000, Remark);
          dbParam[6] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
          dbParam[7] = new DbParameter("@CityId", DbParameter.DbType.Int, 4, CityId);
          dbParam[8] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
          // dbParam[9] = new DbParameter("@nCityId", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);

          dbParam[9] = new DbParameter("@nCityId", DbParameter.DbType.Int, 4, nCityId);
          dbParam[10] = new DbParameter("@frTime1", DbParameter.DbType.VarChar, 10, frTime1);
          dbParam[11] = new DbParameter("@frTime2", DbParameter.DbType.VarChar, 10, frTime2);
          dbParam[12] = new DbParameter("@toTime1", DbParameter.DbType.VarChar, 10, toTime1);
          dbParam[13] = new DbParameter("@toTime2", DbParameter.DbType.VarChar, 10, toTime2);
          dbParam[14] = new DbParameter("@WithUserId", DbParameter.DbType.Int, 4, WithUserId);
          dbParam[15] = new DbParameter("@ModeOfTransport", DbParameter.DbType.VarChar, 25, ModeOfTransport);
          dbParam[16] = new DbParameter("@VehicleUsed", DbParameter.DbType.VarChar, 25, VehicleUsed);
          dbParam[17] = new DbParameter("@Industry", DbParameter.DbType.VarChar, 4000, Industry);
          dbParam[18] = new DbParameter("@Lock", DbParameter.DbType.Bit, 150, Lock);
          dbParam[19] = new DbParameter("@nWithUserId", DbParameter.DbType.Int, 80, nWithUserId);
          dbParam[20] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 4000, "");
          dbParam[21] = new DbParameter("@AppBy", DbParameter.DbType.Int, 4, 0);
          dbParam[22] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 4000, "");


          
          dbParam[23] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
          dbParam[24] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "s_TransVisit_ups", dbParam);
          return Convert.ToInt32(dbParam[24].Value);
      }
      public int InsertVisit(string VisitDocId, int UserId, string VDate, string NextVisitDate, string Remark, int SMId, int CityId, int DistId, int nCityId, string frTime1, string frTime2, string toTime1, string toTime2, int WithUserId, string ModeOfTransport, string VehicleUsed, string Industry, bool Lock, int nWithUserId, string citys, string CityStrtext, decimal OrderAmountEmail, decimal OrderAmountPhone)
      {
          DbParameter[] dbParam = new DbParameter[29];
          dbParam[0] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, 0);
          dbParam[1] = new DbParameter("@VisitDocId", DbParameter.DbType.VarChar, 30, VisitDocId);
          dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
          dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          if (NextVisitDate == "")
          {
              dbParam[4] = new DbParameter("@NextVisitDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[4] = new DbParameter("@NextVisitDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(NextVisitDate));
          }
          dbParam[5] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 4000, Remark);
          dbParam[6] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
          dbParam[7] = new DbParameter("@CityId", DbParameter.DbType.Int, 4, CityId);
          dbParam[8] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
          // dbParam[9] = new DbParameter("@nCityId", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);

          dbParam[9] = new DbParameter("@nCityId", DbParameter.DbType.Int, 4, nCityId);
          dbParam[10] = new DbParameter("@frTime1", DbParameter.DbType.VarChar, 10, frTime1);
          dbParam[11] = new DbParameter("@frTime2", DbParameter.DbType.VarChar, 10, frTime2);
          dbParam[12] = new DbParameter("@toTime1", DbParameter.DbType.VarChar, 10, toTime1);
          dbParam[13] = new DbParameter("@toTime2", DbParameter.DbType.VarChar, 10, toTime2);
          dbParam[14] = new DbParameter("@WithUserId", DbParameter.DbType.Int, 4, WithUserId);
          dbParam[15] = new DbParameter("@ModeOfTransport", DbParameter.DbType.VarChar, 25, ModeOfTransport);
          dbParam[16] = new DbParameter("@VehicleUsed", DbParameter.DbType.VarChar, 25, VehicleUsed);
          dbParam[17] = new DbParameter("@Industry", DbParameter.DbType.VarChar, 4000, Industry);
          dbParam[18] = new DbParameter("@Lock", DbParameter.DbType.Bit, 150, Lock);
          dbParam[19] = new DbParameter("@nWithUserId", DbParameter.DbType.Int, 80, nWithUserId);
          dbParam[20] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 4000, "");
          dbParam[21] = new DbParameter("@AppBy", DbParameter.DbType.Int, 4, 0);
          dbParam[22] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 4000, "");
          dbParam[23] = new DbParameter("@nCityIds", DbParameter.DbType.VarChar, 4000, citys);
          dbParam[24] = new DbParameter("@CityStrtext", DbParameter.DbType.VarChar, 4000, CityStrtext);
          dbParam[25] = new DbParameter("@OrderAmountEmail", DbParameter.DbType.Decimal, 20, OrderAmountEmail);
          dbParam[26] = new DbParameter("@OrderAmountPhone", DbParameter.DbType.Decimal, 20, OrderAmountPhone);

          dbParam[27] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
          dbParam[28] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "s_TransVisit_ups", dbParam);
          return Convert.ToInt32(dbParam[28].Value);
      }

      //public int Insert(string VisitDocId, int UserId, string VDate, string Remark, int SMId, string frTime1,string toTime1, string ModeOfTransport, string VehicleUsed, bool Lock)
      //{
      //    DbParameter[] dbParam = new DbParameter[13];
      //    dbParam[0] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, 0);
      //    dbParam[1] = new DbParameter("@VisitDocId", DbParameter.DbType.VarChar, 30, VisitDocId);
      //    dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
      //    dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
      //    dbParam[4] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 4000, Remark);
      //    dbParam[5] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
      //    dbParam[6] = new DbParameter("@frTime1", DbParameter.DbType.VarChar, 10, frTime1);
      //    dbParam[7] = new DbParameter("@toTime1", DbParameter.DbType.VarChar, 10, toTime1);
      //    dbParam[8] = new DbParameter("@ModeOfTransport", DbParameter.DbType.VarChar, 25, ModeOfTransport);
      //    dbParam[9] = new DbParameter("@VehicleUsed", DbParameter.DbType.VarChar, 25, VehicleUsed);
      //    dbParam[10] = new DbParameter("@Lock", DbParameter.DbType.Bit, 150, Lock);
      //    dbParam[11] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
      //    dbParam[12] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
      //    DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "s_TransVisit_ups", dbParam);
      //    return Convert.ToInt32(dbParam[12].Value);
      //}

      public int Update(Int64 VisId, int UserId, string VDate, string NextVisitDate, string Remark, int SMId, int CityId, int DistId, int nCityId, string frTime1, string frTime2, string toTime1, string toTime2, int WithUserId, string ModeOfTransport, string VehicleUsed, string Industry, int nWithUserId)
      {
          DbParameter[] dbParam = new DbParameter[25];
          dbParam[0] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
          dbParam[1] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);

          if (NextVisitDate == "")
          {
              dbParam[2] = new DbParameter("@NextVisitDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {

              dbParam[2] = new DbParameter("@NextVisitDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(NextVisitDate));
          }
          dbParam[3] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 4000, Remark);
          dbParam[4] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
          dbParam[5] = new DbParameter("@CityId", DbParameter.DbType.Int, 4, CityId);
          dbParam[6] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
          dbParam[7] = new DbParameter("@nCityId", DbParameter.DbType.Int, 4, nCityId);
          dbParam[8] = new DbParameter("@frTime1", DbParameter.DbType.VarChar, 10, frTime1);
          dbParam[9] = new DbParameter("@frTime2", DbParameter.DbType.VarChar, 10, frTime2);
          dbParam[10] = new DbParameter("@toTime1", DbParameter.DbType.VarChar, 10, toTime1);
          dbParam[11] = new DbParameter("@toTime2", DbParameter.DbType.VarChar, 10, toTime2);
          dbParam[12] = new DbParameter("@WithUserId", DbParameter.DbType.Int, 4, WithUserId);
          dbParam[13] = new DbParameter("@ModeOfTransport", DbParameter.DbType.VarChar, 25, ModeOfTransport);
          dbParam[14] = new DbParameter("@VehicleUsed", DbParameter.DbType.VarChar, 25, VehicleUsed);
          dbParam[15] = new DbParameter("@Industry", DbParameter.DbType.VarChar, 4000, Industry);
          dbParam[16] = new DbParameter("@nWithUserId", DbParameter.DbType.Int, 80, nWithUserId);

          dbParam[17] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 4000, "");
          dbParam[18] = new DbParameter("@AppBy", DbParameter.DbType.Int, 4, 0);
          dbParam[19] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 4000, "");

          dbParam[20] = new DbParameter("@VisitDocId", DbParameter.DbType.VarChar, 30, "");
          if (VDate == "")
          {
            dbParam[21] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[21] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          }

          dbParam[22] = new DbParameter("@Lock", DbParameter.DbType.Bit, 150, false);
         
          dbParam[23] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
          dbParam[24] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
         
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "s_TransVisit_ups", dbParam);
          return Convert.ToInt32(dbParam[24].Value);
      }
      public int UpdateVisit(Int64 VisId, int UserId, string VDate, string NextVisitDate, string Remark, int SMId, int CityId, int DistId, int nCityId, string frTime1, string frTime2, string toTime1, string toTime2, int WithUserId, string ModeOfTransport, string VehicleUsed, string Industry, int nWithUserId, string citys, string CityStrtext, decimal OrderAmountEmail, decimal OrderAmountPhone)
      {
          DbParameter[] dbParam = new DbParameter[29];
          dbParam[0] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
          dbParam[1] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);

          if (NextVisitDate == "")
          {
              dbParam[2] = new DbParameter("@NextVisitDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {

              dbParam[2] = new DbParameter("@NextVisitDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(NextVisitDate));
          }
          dbParam[3] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 4000, Remark);
          dbParam[4] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
          dbParam[5] = new DbParameter("@CityId", DbParameter.DbType.Int, 4, CityId);
          dbParam[6] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
          dbParam[7] = new DbParameter("@nCityId", DbParameter.DbType.Int, 4, nCityId);
          dbParam[8] = new DbParameter("@frTime1", DbParameter.DbType.VarChar, 10, frTime1);
          dbParam[9] = new DbParameter("@frTime2", DbParameter.DbType.VarChar, 10, frTime2);
          dbParam[10] = new DbParameter("@toTime1", DbParameter.DbType.VarChar, 10, toTime1);
          dbParam[11] = new DbParameter("@toTime2", DbParameter.DbType.VarChar, 10, toTime2);
          dbParam[12] = new DbParameter("@WithUserId", DbParameter.DbType.Int, 4, WithUserId);
          dbParam[13] = new DbParameter("@ModeOfTransport", DbParameter.DbType.VarChar, 25, ModeOfTransport);
          dbParam[14] = new DbParameter("@VehicleUsed", DbParameter.DbType.VarChar, 25, VehicleUsed);
          dbParam[15] = new DbParameter("@Industry", DbParameter.DbType.VarChar, 4000, Industry);
          dbParam[16] = new DbParameter("@nWithUserId", DbParameter.DbType.Int, 80, nWithUserId);

          dbParam[17] = new DbParameter("@AppStatus", DbParameter.DbType.VarChar, 4000, "");
          dbParam[18] = new DbParameter("@AppBy", DbParameter.DbType.Int, 4, 0);
          dbParam[19] = new DbParameter("@AppRemark", DbParameter.DbType.VarChar, 4000, "");

          dbParam[20] = new DbParameter("@VisitDocId", DbParameter.DbType.VarChar, 30, "");
          if (VDate == "")
          {
              dbParam[21] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[21] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          }

          dbParam[22] = new DbParameter("@Lock", DbParameter.DbType.Bit, 150, false);
          dbParam[23] = new DbParameter("@nCityIds", DbParameter.DbType.VarChar, 4000, citys);
          dbParam[24] = new DbParameter("@CityStrtext", DbParameter.DbType.VarChar, 4000, CityStrtext);
          dbParam[25] = new DbParameter("@OrderAmountEmail", DbParameter.DbType.Decimal, 20, OrderAmountEmail);
          dbParam[26] = new DbParameter("@OrderAmountPhone", DbParameter.DbType.Decimal, 20, OrderAmountPhone);

          dbParam[27] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
          dbParam[28] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "s_TransVisit_ups", dbParam);
          return Convert.ToInt32(dbParam[28].Value);
      }

      public int delete(string VisId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@VisId", DbParameter.DbType.Int, 1, Convert.ToInt32(VisId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransVisit_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }


      public int InsertDiscussionWithDistributor(int UserId, Int64 VisId, int Sno, string VDate, int cityId, int SMId, int DistId, int areaId, string remarkDist, string remarkArea, string remarkL1, string NextVisitDate, string NextVisitTime, string SpentfrTime, string SpentToTime,string imgUrl,string docid)
      {
          DbParameter[] dbParam = new DbParameter[20];
          dbParam[0] = new DbParameter("@VisDistId", DbParameter.DbType.Int, 4, 0);
          dbParam[1] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
          dbParam[2] = new DbParameter("@VisId", DbParameter.DbType.Int, 8, VisId);
          dbParam[3] = new DbParameter("@Sno", DbParameter.DbType.Int, 4, Sno);
          dbParam[4] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          dbParam[5] = new DbParameter("@cityId", DbParameter.DbType.VarChar, 4000, cityId);
          dbParam[6] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
          dbParam[7] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
          dbParam[8] = new DbParameter("@areaId", DbParameter.DbType.Int, 4, areaId);
          dbParam[9] = new DbParameter("@remarkDist", DbParameter.DbType.VarChar, 4000, remarkDist);
          dbParam[10] = new DbParameter("@remarkArea", DbParameter.DbType.VarChar, 10, remarkArea);
          dbParam[11] = new DbParameter("@remarkL1", DbParameter.DbType.VarChar, 10, remarkL1);
          dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
          dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          dbParam[14] = new DbParameter("@NextVisitDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(NextVisitDate));
          dbParam[15] = new DbParameter("@NextVisitTime", DbParameter.DbType.VarChar, 10, NextVisitTime);
          dbParam[16] = new DbParameter("@SpentfrTime", DbParameter.DbType.VarChar, 10, SpentfrTime);
          dbParam[17] = new DbParameter("@SpentToTime", DbParameter.DbType.VarChar, 10, SpentToTime);
          dbParam[18] = new DbParameter("@ImgUrl", DbParameter.DbType.VarChar, 255, imgUrl);
          dbParam[19] = new DbParameter("@DocId ", DbParameter.DbType.VarChar, 30, docid);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransVisitDist_ups", dbParam);
          return Convert.ToInt32(dbParam[13].Value);

    
      }
      public int UpdateDiscussionWithDistributor(Int64 visitDistID, int UserId, Int64 VisId, int Sno, string VDate, int cityId, int SMId, int DistId, int areaId, string remarkDist, string remarkArea, string remarkL1, string NextVisitDate, string NextVisitTime, string SpentfrTime, string SpentToTime, string imgUrl)
      {
          DbParameter[] dbParam = new DbParameter[20];

          dbParam[0] = new DbParameter("@VisDistId", DbParameter.DbType.Int, 4, visitDistID);
          dbParam[1] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
          dbParam[2] = new DbParameter("@VisId", DbParameter.DbType.Int, 8, VisId);
          dbParam[3] = new DbParameter("@Sno", DbParameter.DbType.Int, 4, Sno);
          dbParam[4] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          dbParam[5] = new DbParameter("@cityId", DbParameter.DbType.VarChar, 4000, cityId);
          dbParam[6] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
          dbParam[7] = new DbParameter("@DistId", DbParameter.DbType.Int, 4, DistId);
          dbParam[8] = new DbParameter("@areaId", DbParameter.DbType.Int, 4, areaId);
          dbParam[9] = new DbParameter("@remarkDist", DbParameter.DbType.VarChar, 4000, remarkDist);
          dbParam[10] = new DbParameter("@remarkArea", DbParameter.DbType.VarChar, 10, "");
          dbParam[11] = new DbParameter("@remarkL1", DbParameter.DbType.VarChar, 10, "");         
          dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
          dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          dbParam[14] = new DbParameter("@NextVisitDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(NextVisitDate));
          dbParam[15] = new DbParameter("@NextVisitTime", DbParameter.DbType.VarChar, 10, NextVisitTime);
          dbParam[16] = new DbParameter("@SpentfrTime", DbParameter.DbType.VarChar, 10, SpentfrTime);
          dbParam[17] = new DbParameter("@SpentToTime", DbParameter.DbType.VarChar, 10, SpentToTime);
          dbParam[18] = new DbParameter("@ImgUrl", DbParameter.DbType.VarChar, 255, imgUrl);
          dbParam[19] = new DbParameter("@DocId ", DbParameter.DbType.VarChar, 30, "");
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransVisitDist_ups", dbParam);
          return Convert.ToInt32(dbParam[13].Value);
      }

      public int deleteDiscussionWithDistributor(string VisDistId)
      {
          DbParameter[] dbParam = new DbParameter[2];
          dbParam[0] = new DbParameter("@VisDistId", DbParameter.DbType.Int, 1, Convert.ToInt32(VisDistId));
          dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransVisitDist_del", dbParam);
          return Convert.ToInt32(dbParam[1].Value);
      }
    
      public int UpdateDiscussionWithL1(int visitDistID,string remarkL1)
      {
          DbParameter[] dbParam = new DbParameter[3];
          dbParam[0] = new DbParameter("@remarkL1", DbParameter.DbType.VarChar,4000, remarkL1);
          dbParam[1] = new DbParameter("@VisDistId", DbParameter.DbType.Int, 4, visitDistID);
          dbParam[2] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransVisitDisL1_ups", dbParam);
          return Convert.ToInt32(dbParam[2].Value);
      }
      public int UpdateAreaPlan(int visitDistID, string remarkArea)
      {
          DbParameter[] dbParam = new DbParameter[3];
          dbParam[0] = new DbParameter("@remarkArea", DbParameter.DbType.VarChar, 4000, remarkArea);
          dbParam[1] = new DbParameter("@VisDistId", DbParameter.DbType.Int, 4, visitDistID);
          dbParam[2] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransVisitDistArea_ups", dbParam);
          return Convert.ToInt32(dbParam[2].Value);
      }

      public int UpdateDist(int visitDistID, string dist)
      {
          DbParameter[] dbParam = new DbParameter[3];
          dbParam[0] = new DbParameter("@remarkDist", DbParameter.DbType.VarChar, 4000, dist);
          dbParam[1] = new DbParameter("@VisDistId", DbParameter.DbType.Int, 4, visitDistID);
          dbParam[2] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransDist_ups", dbParam);
          return Convert.ToInt32(dbParam[2].Value);
      }

      public int InsertFaildVisit(Int64 VisId, string FVDocId, string VDate, int UserID, int SMId, int PartyId, string Remarks, string Nextvisit, int ReasonID, string VisitTime,string AreaId)
      {
          DbParameter[] dbParam = new DbParameter[14];
          dbParam[0] = new DbParameter("@FVId", DbParameter.DbType.Int, 4, 0);
          dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.Int, 8, VisId);
          dbParam[2] = new DbParameter("@FVDocId", DbParameter.DbType.VarChar, 30, FVDocId);
          dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          dbParam[4] = new DbParameter("@UserID", DbParameter.DbType.Int, 4, UserID);
          dbParam[5] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
          dbParam[6] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
          dbParam[7] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 255, Remarks);
          dbParam[8] = new DbParameter("@Nextvisit", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(Nextvisit));
          dbParam[9] = new DbParameter("@ReasonID", DbParameter.DbType.Int, 4, ReasonID);
          dbParam[10] = new DbParameter("@VisitTime", DbParameter.DbType.VarChar, 50, VisitTime);

          dbParam[11] = new DbParameter("@AreaId", DbParameter.DbType.Int,4, AreaId);

          dbParam[12] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
          dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransFailedVisit_ups", dbParam);
          return Convert.ToInt32(dbParam[13].Value);
      }

      public int UpdateFaildVisit(int FVId, Int64 VisId, string VDate, int UserID, int SMId, int PartyId, string Remarks, string Nextvisit, int ReasonID, string VisitTime, string AreaId)
      {
          DbParameter[] dbParam = new DbParameter[14];
          dbParam[0] = new DbParameter("@FVId", DbParameter.DbType.Int, 4, FVId);
          dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.Int, 8, VisId);
          dbParam[2] = new DbParameter("@FVDocId", DbParameter.DbType.VarChar, 30, "");
          dbParam[3] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          dbParam[4] = new DbParameter("@UserID", DbParameter.DbType.Int, 4, UserID);
          dbParam[5] = new DbParameter("@SMId", DbParameter.DbType.Int, 4, SMId);
          dbParam[6] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
          dbParam[7] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 255, Remarks);
          dbParam[8] = new DbParameter("@Nextvisit", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(Nextvisit));
          dbParam[9] = new DbParameter("@ReasonID", DbParameter.DbType.Int, 4, ReasonID);
          dbParam[10] = new DbParameter("@VisitTime", DbParameter.DbType.VarChar, 50, VisitTime);
          dbParam[11] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
          dbParam[12] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          dbParam[13] = new DbParameter("@AreaId", DbParameter.DbType.Int, 4, AreaId);

          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Temp_TransFailedVisit_ups", dbParam);
          return Convert.ToInt32(dbParam[12].Value);
      }
    

      
    }
}
