using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;

namespace BAL.Order
{
  public class OrderEntryBAL
    {
        //	@ord1id bigint,@Sno int,@itemid int,@qty numeric(18, 2),@freeqty numeric(18, 2) ,@rate numeric(18, 2),@discount numeric(18, 2),
      public int InsertOrderEntry(Int64 VisId, string OrdDocId, int UserId, string VDate, int SMId, int PartyId, int AreaId, string Remarks, decimal OrderAmount)
      {
          DbParameter[] dbParam = new DbParameter[13];


          dbParam[0] = new DbParameter("@OrdId", DbParameter.DbType.Int, 4, 0);
          dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
          dbParam[2] = new DbParameter("@OrdDocId", DbParameter.DbType.VarChar, 30, OrdDocId);
          dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
          dbParam[4] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
          dbParam[5] = new DbParameter("@SMId", DbParameter.DbType.BigInt, 8, SMId);
          dbParam[6] = new DbParameter("@AreaId", DbParameter.DbType.VarChar, 20, AreaId);
          if (VDate == "")
          {
              dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          }
          dbParam[8] = new DbParameter("@OrderAmount", DbParameter.DbType.Decimal, 20, OrderAmount);
          dbParam[9] = new DbParameter("@OrderStatus", DbParameter.DbType.VarChar, 20, 0);


          dbParam[10] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 150, Remarks);
          dbParam[11] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
          dbParam[12] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 4, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "udp_Temp_TransOrder_ups", dbParam);
          return Convert.ToInt32(dbParam[12].Value);
      }

      public int InsertOrderEntryItemWise(Int64 VisId, string OrdDocId, int UserId, string VDate, int SMId, int PartyId, int AreaId, int ord1id, int Sno, int itemid, decimal qty, decimal freeqty, decimal rate, decimal discount, string Remarks, decimal OrderAmount, decimal cases, decimal @Unit)   	
      {
          DbParameter[] dbParam = new DbParameter[22];


          dbParam[0] = new DbParameter("@OrdId", DbParameter.DbType.Int, 4, 0);
          dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
          dbParam[2] = new DbParameter("@OrdDocId", DbParameter.DbType.VarChar, 30, OrdDocId);
          dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
          dbParam[4] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
          dbParam[5] = new DbParameter("@SMId", DbParameter.DbType.BigInt, 8, SMId);
          dbParam[6] = new DbParameter("@AreaId", DbParameter.DbType.VarChar, 20, AreaId);
          if (VDate == "")
          {
              dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          }
          dbParam[8] = new DbParameter("@OrderAmount", DbParameter.DbType.Decimal, 20, OrderAmount);
          dbParam[9] = new DbParameter("@OrderStatus", DbParameter.DbType.VarChar, 20, 0);


          dbParam[10] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 150, Remarks);
          dbParam[11] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
          dbParam[12] = new DbParameter("@ord1id", DbParameter.DbType.BigInt, 15, ord1id);
          dbParam[13] = new DbParameter("@Sno", DbParameter.DbType.Int, 4, 1);
          dbParam[14] = new DbParameter("@itemid", DbParameter.DbType.Int, 4, itemid);
          dbParam[15] = new DbParameter("@qty", DbParameter.DbType.Decimal, 20, qty);
          dbParam[16] = new DbParameter("@freeqty", DbParameter.DbType.Decimal, 20, 0.00);
          dbParam[17] = new DbParameter("@rate", DbParameter.DbType.Decimal, 10, rate);
          dbParam[18] = new DbParameter("@discount", DbParameter.DbType.Decimal, 20, 0.00);
          dbParam[19] = new DbParameter("@cases", DbParameter.DbType.Decimal, 20, cases);
          dbParam[20] = new DbParameter("@Unit", DbParameter.DbType.Decimal, 20,Unit);
  

          dbParam[21] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 4, ParameterDirection.Output);    
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "udp_Temp_TransOrder_ups_itemWise", dbParam);
          return Convert.ToInt32(dbParam[21].Value);
      }



      public int InsertReturnEntryItemWise(Int64 VisId, string OrdDocId, int UserId, string VDate, int SMId, int PartyId, int AreaId, int ord1id, int Sno, int itemid, decimal qty, decimal freeqty, decimal rate, decimal discount, string Remarks, decimal OrderAmount, decimal cases, decimal @Unit, string BatchNo, string MFD)
      {
          DbParameter[] dbParam = new DbParameter[24];


          dbParam[0] = new DbParameter("@SRetId", DbParameter.DbType.Int, 4, 0);
          dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
          dbParam[2] = new DbParameter("@SRetDocId", DbParameter.DbType.VarChar, 30, OrdDocId);
          dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
          dbParam[4] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
          dbParam[5] = new DbParameter("@SMId", DbParameter.DbType.BigInt, 8, SMId);
          dbParam[6] = new DbParameter("@AreaId", DbParameter.DbType.VarChar, 20, AreaId);
          if (VDate == "")
          {
              dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          }
          dbParam[8] = new DbParameter("@OrderAmount", DbParameter.DbType.Decimal, 20, OrderAmount);
          dbParam[9] = new DbParameter("@OrderStatus", DbParameter.DbType.VarChar, 20, 0);


          dbParam[10] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 150, Remarks);
          dbParam[11] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
          dbParam[12] = new DbParameter("@SRet1Id", DbParameter.DbType.BigInt, 15, ord1id);
          dbParam[13] = new DbParameter("@Sno", DbParameter.DbType.Int, 4, 1);
          dbParam[14] = new DbParameter("@itemid", DbParameter.DbType.Int, 4, itemid);
          dbParam[15] = new DbParameter("@qty", DbParameter.DbType.Decimal, 20, qty);
          dbParam[16] = new DbParameter("@freeqty", DbParameter.DbType.Decimal, 20, 0.00);
          dbParam[17] = new DbParameter("@rate", DbParameter.DbType.Decimal, 10, rate);
          dbParam[18] = new DbParameter("@discount", DbParameter.DbType.Decimal, 20, 0.00);
          dbParam[19] = new DbParameter("@cases", DbParameter.DbType.Decimal, 20, cases);
          dbParam[20] = new DbParameter("@Unit", DbParameter.DbType.Decimal, 20, Unit);

          dbParam[21] = new DbParameter("@BatchNo", DbParameter.DbType.VarChar, 50, BatchNo);

          if (MFD == "")
          {
              dbParam[22] = new DbParameter("@MFD", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[22] = new DbParameter("@MFD", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(MFD));
          }
          dbParam[23] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 4, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "udp_Temp_TransSalesReturn_ups_itemWise", dbParam);
          return Convert.ToInt32(dbParam[23].Value);
      }




      public int UpdateOrderEntryItemWise(Int64 OrdId, Int64 VisId, int UserId, string VDate, int SMId, int PartyId, int AreaId, string Remarks, decimal OrderAmount, int ord1id, int itemid, decimal qty, decimal rate, decimal cases, decimal @Unit)
      {
          DbParameter[] dbParam = new DbParameter[22];


          dbParam[0] = new DbParameter("@OrdId", DbParameter.DbType.Int, 4, OrdId);
          dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
          dbParam[2] = new DbParameter("@OrdDocId", DbParameter.DbType.VarChar, 30, "");
          dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
          dbParam[4] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
          dbParam[5] = new DbParameter("@SMId", DbParameter.DbType.BigInt, 8, SMId);
          dbParam[6] = new DbParameter("@AreaId", DbParameter.DbType.VarChar, 20, AreaId);
          if (VDate == "")
          {
              dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          }
          dbParam[8] = new DbParameter("@OrderAmount", DbParameter.DbType.Decimal, 20, OrderAmount);
          dbParam[9] = new DbParameter("@OrderStatus", DbParameter.DbType.VarChar, 20, 0);
          dbParam[10] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 150, Remarks);
          dbParam[11] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
          dbParam[12] = new DbParameter("@ord1id", DbParameter.DbType.BigInt, 15, ord1id);
          dbParam[13] = new DbParameter("@Sno", DbParameter.DbType.Int, 4, 0);
          dbParam[14] = new DbParameter("@itemid", DbParameter.DbType.Int, 4, itemid);
          dbParam[15] = new DbParameter("@qty", DbParameter.DbType.Decimal, 20, qty);
          dbParam[16] = new DbParameter("@freeqty", DbParameter.DbType.Decimal, 20, 0.00);
          dbParam[17] = new DbParameter("@rate", DbParameter.DbType.Decimal, 10, rate);
          dbParam[18] = new DbParameter("@discount", DbParameter.DbType.Decimal, 20, 0.00);
          dbParam[19] = new DbParameter("@cases", DbParameter.DbType.Decimal, 20, cases);
          dbParam[20] = new DbParameter("@Unit", DbParameter.DbType.Decimal, 20, Unit);
          dbParam[21] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 4, ParameterDirection.Output);    
  
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "udp_Temp_TransOrder_ups_itemWise", dbParam);
          return Convert.ToInt32(dbParam[12].Value);
      }




      public int UpdateReturnEntryItemWise(Int64 SRetId, Int64 VisId, int UserId, string VDate, int SMId, int PartyId, int AreaId, string Remarks, decimal OrderAmount, int SRet1Id, int itemid, decimal qty, decimal rate, decimal cases, decimal @Unit, string BatchNo, string MFD)
      {

 
          DbParameter[] dbParam = new DbParameter[24];


          dbParam[0] = new DbParameter("@SRetId", DbParameter.DbType.Int, 4, SRetId);
          dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
          dbParam[2] = new DbParameter("@SRetDocId", DbParameter.DbType.VarChar, 30, "");
          dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
          dbParam[4] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
          dbParam[5] = new DbParameter("@SMId", DbParameter.DbType.BigInt, 8, SMId);
          dbParam[6] = new DbParameter("@AreaId", DbParameter.DbType.VarChar, 20, AreaId);
          if (VDate == "")
          {
              dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          }
          dbParam[8] = new DbParameter("@OrderAmount", DbParameter.DbType.Decimal, 20, OrderAmount);
          dbParam[9] = new DbParameter("@OrderStatus", DbParameter.DbType.VarChar, 20, 0);
          dbParam[10] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 150, Remarks);
          dbParam[11] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
          dbParam[12] = new DbParameter("@SRet1Id", DbParameter.DbType.BigInt, 15, SRet1Id);
          dbParam[13] = new DbParameter("@Sno", DbParameter.DbType.Int, 4, 0);
          dbParam[14] = new DbParameter("@itemid", DbParameter.DbType.Int, 4, itemid);
          dbParam[15] = new DbParameter("@qty", DbParameter.DbType.Decimal, 20, qty);
          dbParam[16] = new DbParameter("@freeqty", DbParameter.DbType.Decimal, 20, 0.00);
          dbParam[17] = new DbParameter("@rate", DbParameter.DbType.Decimal, 10, rate);
          dbParam[18] = new DbParameter("@discount", DbParameter.DbType.Decimal, 20, 0.00);
          dbParam[19] = new DbParameter("@cases", DbParameter.DbType.Decimal, 20, cases);
          dbParam[20] = new DbParameter("@Unit", DbParameter.DbType.Decimal, 20, Unit);
            dbParam[21] = new DbParameter("@BatchNo", DbParameter.DbType.VarChar, 50, BatchNo);

             if (MFD == "")
          {
              dbParam[22] = new DbParameter("@MFD", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[22] = new DbParameter("@MFD", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(MFD));
          }
          dbParam[23] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 4, ParameterDirection.Output);

          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "udp_Temp_TransSalesReturn_ups_itemWise", dbParam);
          return Convert.ToInt32(dbParam[12].Value);
      }


      public int UpdateOrderEntry(Int64 OrdId,Int64 VisId,int UserId, string VDate, int SMId, int PartyId, int AreaId, string Remarks, decimal OrderAmount)
      {
          DbParameter[] dbParam = new DbParameter[13];
          dbParam[0] = new DbParameter("@OrdId", DbParameter.DbType.Int, 4, OrdId);
          dbParam[1] = new DbParameter("@VisId", DbParameter.DbType.Int, 4, VisId);
          dbParam[2] = new DbParameter("@OrdDocId", DbParameter.DbType.VarChar, 30, "");
          dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 4, UserId);
          dbParam[4] = new DbParameter("@PartyId", DbParameter.DbType.Int, 4, PartyId);
          dbParam[5] = new DbParameter("@SMId", DbParameter.DbType.BigInt, 8, SMId);
          dbParam[6] = new DbParameter("@AreaId", DbParameter.DbType.VarChar, 20, AreaId);
          if (VDate == "")
          {
              dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
          }
          else
          {
              dbParam[7] = new DbParameter("@VDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(VDate));
          }
          dbParam[8] = new DbParameter("@OrderAmount", DbParameter.DbType.Decimal, 20, OrderAmount);
          dbParam[9] = new DbParameter("@OrderStatus", DbParameter.DbType.VarChar, 20, 0);
          dbParam[10] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 150, Remarks);
          dbParam[11] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Update");
          dbParam[12] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 4, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "udp_Temp_TransOrder_ups", dbParam);
          return Convert.ToInt32(dbParam[12].Value);
      }

      public int delete(string OrdId)
      {
          DbParameter[] dbParam = new DbParameter[2];
          dbParam[0] = new DbParameter("@OrdId", DbParameter.DbType.Int, 1, Convert.ToInt32(OrdId));
          dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "udp_Temp_TransOrder_del", dbParam);
          return Convert.ToInt32(dbParam[1].Value);
      }
      public int delete_itemwise(string ord1id, string ordid)
      {

          DbParameter[] dbParam = new DbParameter[3];
          dbParam[0] = new DbParameter("@Ord1Id", DbParameter.DbType.Int, 1, Convert.ToInt32(ord1id));
          dbParam[1] = new DbParameter("@OrdId", DbParameter.DbType.Int, 1, Convert.ToInt32(ordid));
          dbParam[2] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "udp_Temp_TransOrder_del_itemwise", dbParam);
          return Convert.ToInt32(dbParam[1].Value);

      }
      public int delete_itemwiseSaleReturn(string ord1id, string ordid)
      {

          DbParameter[] dbParam = new DbParameter[3];
          dbParam[0] = new DbParameter("@SRet1Id", DbParameter.DbType.Int, 1, Convert.ToInt32(ord1id));
          dbParam[1] = new DbParameter("@SRetId", DbParameter.DbType.Int, 1, Convert.ToInt32(ordid));
          dbParam[2] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "udp_Temp_TransSalesReturn_del_itemwise", dbParam);
          return Convert.ToInt32(dbParam[1].Value);

      }
    }
}
