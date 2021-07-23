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
  public class ItemBAL
    {

      public int InsertItems(string ParentName, string ItemName, string ItemCode, string Unit, string Active, string StdPack, string Syncid, decimal Mrp, decimal Dp, decimal Rp, string ProductClass, string Segment, string PriceGroup, string primaryunit, string Secondaryunit, decimal PrimaryUnitfactor, decimal SecondaryUnitfactor, decimal MOQ, string Promoted, decimal cgstper, decimal sgstper, decimal igstper)
      {
          DbParameter[] dbParam = new DbParameter[23];
          dbParam[0] = new DbParameter("@UnderID", DbParameter.DbType.VarChar, 50, ParentName);
          dbParam[1] = new DbParameter("@ItemName", DbParameter.DbType.VarChar, 50, ItemName);
          dbParam[2] = new DbParameter("@ItemCode", DbParameter.DbType.VarChar, 50, ItemCode);
          dbParam[3] = new DbParameter("@Unit", DbParameter.DbType.VarChar, 10, Unit);
          dbParam[4] = new DbParameter("@Active", DbParameter.DbType.VarChar, 1, Active);
          dbParam[5] = new DbParameter("@StdPack", DbParameter.DbType.VarChar, 30, StdPack);
          dbParam[6] = new DbParameter("@Syncid", DbParameter.DbType.VarChar, 150, Syncid);
          dbParam[7] = new DbParameter("@Mrp", DbParameter.DbType.Decimal, 12, Mrp);
          dbParam[8] = new DbParameter("@Dp", DbParameter.DbType.Decimal, 12, Dp);
          dbParam[9] = new DbParameter("@Rp", DbParameter.DbType.Decimal, 12, Rp);
          dbParam[10] = new DbParameter("@ClassId", DbParameter.DbType.VarChar, 10, ProductClass);
          dbParam[11] = new DbParameter("@SegmentId", DbParameter.DbType.VarChar, 10, Segment);
          dbParam[12] = new DbParameter("@PriceGroup", DbParameter.DbType.VarChar, 20, PriceGroup);
          dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

          dbParam[14] = new DbParameter("@primaryunit", DbParameter.DbType.VarChar, 20, primaryunit);
          dbParam[15] = new DbParameter("@Secondaryunit", DbParameter.DbType.VarChar, 20, Secondaryunit);
          dbParam[16] = new DbParameter("@PrimaryUnitfactor", DbParameter.DbType.Decimal, 12, PrimaryUnitfactor);

          dbParam[17] = new DbParameter("@SecondaryUnitfactor", DbParameter.DbType.Decimal, 12, SecondaryUnitfactor);

          dbParam[18] = new DbParameter("@MOQ", DbParameter.DbType.Decimal, 12, MOQ);
          dbParam[19] = new DbParameter("@Promoted", DbParameter.DbType.VarChar, 1, Promoted);
          dbParam[20] = new DbParameter("@cgstper", DbParameter.DbType.Decimal, 12, cgstper);
          dbParam[21] = new DbParameter("@sgstper", DbParameter.DbType.Decimal, 12, sgstper);
          dbParam[22] = new DbParameter("@igstper", DbParameter.DbType.Decimal, 12, igstper);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertItemForm", dbParam);
          return Convert.ToInt32(dbParam[13].Value);
      }
      public int UpdateItems(Int32 ItemId, string ParentName, string ItemName, string ItemCode, string Unit, string Active, string StdPack, string Syncid, decimal Mrp, decimal Dp, decimal Rp, string ProductClass, string Segment, string PriceGroup, string primaryunit, string Secondaryunit, decimal PrimaryUnitfactor, decimal SecondaryUnitfactor, decimal MOQ, string Promoted, decimal cgstper, decimal sgstper, decimal igstper)
      {
          DbParameter[] dbParam = new DbParameter[24];
          dbParam[0] = new DbParameter("@ItemId", DbParameter.DbType.Int, 4, ItemId);
          dbParam[1] = new DbParameter("@UnderId", DbParameter.DbType.VarChar, 50, ParentName);
          dbParam[2] = new DbParameter("@ItemName", DbParameter.DbType.VarChar, 50, ItemName);
          dbParam[3] = new DbParameter("@ItemCode", DbParameter.DbType.VarChar, 50, ItemCode);
          dbParam[4] = new DbParameter("@Unit", DbParameter.DbType.VarChar, 10, Unit);
          dbParam[5] = new DbParameter("@Active", DbParameter.DbType.VarChar, 1, Active);
          dbParam[6] = new DbParameter("@StdPack", DbParameter.DbType.VarChar, 30, StdPack);
          dbParam[7] = new DbParameter("@Syncid", DbParameter.DbType.VarChar, 150, Syncid);
          dbParam[8] = new DbParameter("@Mrp", DbParameter.DbType.Decimal, 12, Mrp);
          dbParam[9] = new DbParameter("@Dp", DbParameter.DbType.Decimal, 12, Dp);
          dbParam[10] = new DbParameter("@Rp", DbParameter.DbType.Decimal, 12, Rp);
          dbParam[11] = new DbParameter("@ClassId", DbParameter.DbType.VarChar, 10, ProductClass);
          dbParam[12] = new DbParameter("@SegmentId", DbParameter.DbType.VarChar, 10, Segment);
          dbParam[13] = new DbParameter("@PriceGroup", DbParameter.DbType.VarChar, 20, PriceGroup);
          dbParam[14] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          dbParam[15] = new DbParameter("@primaryunit", DbParameter.DbType.VarChar, 20, primaryunit);
          dbParam[16] = new DbParameter("@Secondaryunit", DbParameter.DbType.VarChar, 20, Secondaryunit);
          dbParam[17] = new DbParameter("@PrimaryUnitfactor", DbParameter.DbType.Decimal, 12, PrimaryUnitfactor);

          dbParam[18] = new DbParameter("@SecondaryUnitfactor", DbParameter.DbType.Decimal, 12, SecondaryUnitfactor);

          dbParam[19] = new DbParameter("@MOQ", DbParameter.DbType.Decimal, 12, MOQ);
          dbParam[20] = new DbParameter("@Promoted", DbParameter.DbType.VarChar, 1, Promoted);
          dbParam[21] = new DbParameter("@cgstper", DbParameter.DbType.Decimal, 12, cgstper);
          dbParam[22] = new DbParameter("@sgstper", DbParameter.DbType.Decimal, 12, sgstper);
          dbParam[23] = new DbParameter("@igstper", DbParameter.DbType.Decimal, 12, igstper);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_UpdateItemForm", dbParam);
          return Convert.ToInt32(dbParam[14].Value);
      }
      public int delete(string ItemId)
      {
          DbParameter[] dbParam = new DbParameter[2];
          dbParam[0] = new DbParameter("@ItemId", DbParameter.DbType.Int, 10, Convert.ToInt32(ItemId));
          dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
          DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteMastItem", dbParam);
          return Convert.ToInt32(dbParam[1].Value);
      }
    }
}
