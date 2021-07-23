using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Text;
using DAL;
namespace AstralFFMS
{

    public class MasterOperation
    {
        string _sql = "";
        public int InsertTransDistInvImport(Int32 DistInvDocId, string VDate, Int32 DistId, decimal taxamt1, decimal BillAmount)
        {
            DbParameter[] dbParam = new DbParameter[6];
            dbParam[0] = new DbParameter("@DistInvDocId", DbParameter.DbType.Int, 100, DistInvDocId);
            dbParam[1] = new DbParameter("@VDate", DbParameter.DbType.VarChar, 100, VDate);
            dbParam[2] = new DbParameter("@DistId", DbParameter.DbType.Int, 100, DistId);
            dbParam[3] = new DbParameter("@taxamt1", DbParameter.DbType.Decimal, 100, taxamt1);
            dbParam[4] = new DbParameter("@BillAmount", DbParameter.DbType.Decimal, 100, BillAmount);
            dbParam[5] = new DbParameter("@DistInvId", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransDistInvImportData", dbParam);
            return Convert.ToInt32(dbParam[5].Value);
        }

        public int InsertTransDistInv1Import(Int32 DistInvId, Int32 DistInvDocId, Int32 Sno, string VDate, Int32 DistId, Int32 ItemID, decimal Qty, decimal Rate, decimal Amount, decimal Tax_Amt, decimal Net_Total, decimal QtyInKg)
        {
            DbParameter[] dbParam = new DbParameter[13];
            dbParam[0] = new DbParameter("@DistInvDocId", DbParameter.DbType.Int, 100, DistInvDocId);
            dbParam[1] = new DbParameter("@VDate", DbParameter.DbType.VarChar, 100, VDate);
            dbParam[2] = new DbParameter("@DistId", DbParameter.DbType.Int, 100, DistId);
            dbParam[3] = new DbParameter("@ItemID", DbParameter.DbType.Int, 100, ItemID);
            dbParam[4] = new DbParameter("@Qty", DbParameter.DbType.Decimal, 100, Qty);
            dbParam[5] = new DbParameter("@Rate", DbParameter.DbType.Decimal, 100, Rate);
            dbParam[6] = new DbParameter("@Amount", DbParameter.DbType.Decimal, 100, Amount);
            dbParam[7] = new DbParameter("@Tax_Amt", DbParameter.DbType.Decimal, 100, Tax_Amt);
            dbParam[8] = new DbParameter("@Net_Total", DbParameter.DbType.Decimal, 100, Net_Total);
            dbParam[9] = new DbParameter("@QtyInKg", DbParameter.DbType.Decimal, 100, QtyInKg);
            dbParam[10] = new DbParameter("@DistInv1Id", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[11] = new DbParameter("@DistInvId", DbParameter.DbType.Int, 100, DistInvId);
            dbParam[12] = new DbParameter("@Sno", DbParameter.DbType.Int, 100, Sno);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransDistInv1ImportData", dbParam);
            return Convert.ToInt32(dbParam[10].Value);
        }
        public void InsertPurchaseOrderImport(DataTable dt)
        {
            DbParameter[] dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@tblUT_PurchaseOrderImport", DbParameter.DbType.Datatable, 200, dt);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Insert_PurchaseOrderImport", dbParam);
        }
        public void CreatePurchaseOrderImport()
        {
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_createUTPurchaseOrder", null);
        }
        public void InsertTransDistributerLedgerImport(DataTable dt)
        {
            DbParameter[] dbParam = new DbParameter[1];
            dbParam[0] = new DbParameter("@tblUT_TransDistributerLedgerImport", DbParameter.DbType.Datatable, 200, dt);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_InsertTransDistributerLedgerImport", dbParam);
        }

    }


}