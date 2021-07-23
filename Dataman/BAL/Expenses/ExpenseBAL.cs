using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;
using System;


namespace BAL
{
   public class ExpenseBAL
   {
       public int InsertUpdateExpenseType(Int32 Id, string Name, string Remarks, string SyncId, bool Active, string ExpenseTypeCode)
       {
           DbParameter[] dbParam = new DbParameter[7];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Id);
           dbParam[1] = new DbParameter("@Name", DbParameter.DbType.VarChar, 100, Name);
           dbParam[2] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 20, SyncId);
           dbParam[3] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 500, Remarks);
           dbParam[4] = new DbParameter("@Active", DbParameter.DbType.Bit, 1, Active);
           dbParam[5] = new DbParameter("@ExpenseTypeCode", DbParameter.DbType.VarChar, 20, ExpenseTypeCode);
           dbParam[6] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsUpdExpenseType", dbParam);
           return Convert.ToInt32(dbParam[6].Value);
       }
       public int delete(string Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 10, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteExpenseType", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
       public int InsertExpenses(Int32 ExpDetailId, string SyncId, int EmployeeId, int ExpenseTypeId, string BillNumber, DateTime BillDate, int FromCity, int ToCity, string FromDate, string ToDate, int CityId, string Remarks, decimal ClaimAmount, decimal BillAmount, int UserId, Boolean IsSupportingAttached, int TravelModeId, int ExpenseGrpId, Boolean StayWithRelative, decimal PerKmRate, decimal kmVisited, string TimeFrom, String TimeTo, string GSTNo, Boolean ischkGstnNo,string vendor, decimal CGSTAmt, decimal SGSTAmt, decimal IGSTAmt)
       {
           DbParameter[] dbParam = new DbParameter[30];
           dbParam[0] = new DbParameter("@ExpDetailId", DbParameter.DbType.Int, 10, ExpDetailId);
           dbParam[1] = new DbParameter("@SyncId", DbParameter.DbType.VarChar, 25, SyncId);
           dbParam[2] = new DbParameter("@EmployeeId", DbParameter.DbType.Int, 10, EmployeeId);
           dbParam[3] = new DbParameter("@ExpenseTypeId", DbParameter.DbType.Int,10, ExpenseTypeId);
           dbParam[4] = new DbParameter("@BillNumber", DbParameter.DbType.VarChar, 20, BillNumber);
           dbParam[5] = new DbParameter("@BillDate", DbParameter.DbType.DateTime, 20, BillDate);
           dbParam[6] = new DbParameter("@FromCity", DbParameter.DbType.Int, 10, FromCity);
           dbParam[7] = new DbParameter("@ToCity", DbParameter.DbType.Int, 10, ToCity);
           if(!string.IsNullOrEmpty(FromDate))
           dbParam[8] = new DbParameter("@FromDate", DbParameter.DbType.DateTime, 20, FromDate);
           else
               dbParam[8] = new DbParameter("@FromDate", DbParameter.DbType.DateTime, 20, DBNull.Value);
           if (!string.IsNullOrEmpty(ToDate))
           dbParam[9] = new DbParameter("@ToDate", DbParameter.DbType.DateTime, 20, ToDate);
           else
           dbParam[9] = new DbParameter("@ToDate", DbParameter.DbType.DateTime, 20, DBNull.Value);
           dbParam[10] = new DbParameter("@CityId", DbParameter.DbType.Int, 10, CityId);
           dbParam[11] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 200, Remarks);
           dbParam[12] = new DbParameter("@ClaimAmount", DbParameter.DbType.Decimal, 15, ClaimAmount);
           dbParam[13] = new DbParameter("@BillAmount", DbParameter.DbType.Decimal, 15, BillAmount);
           dbParam[14] = new DbParameter("@UserId", DbParameter.DbType.Int, 10, UserId);
           dbParam[15] = new DbParameter("@IsSupportingAttached", DbParameter.DbType.Bit, 1, IsSupportingAttached);
           dbParam[16] = new DbParameter("@TravelModeId", DbParameter.DbType.Int, 10, TravelModeId);
           dbParam[17] = new DbParameter("@ExpenseGrpId", DbParameter.DbType.Int, 10, ExpenseGrpId);
           dbParam[18] = new DbParameter("@StayWithRelative", DbParameter.DbType.Bit, 1, StayWithRelative);
           dbParam[19] = new DbParameter("@PerKmRate", DbParameter.DbType.Decimal,20, PerKmRate);
           dbParam[20] = new DbParameter("@kmVisited", DbParameter.DbType.Decimal, 20, kmVisited);
           dbParam[21] = new DbParameter("@TimeFrom", DbParameter.DbType.VarChar, 10, TimeFrom);
           dbParam[22] = new DbParameter("@TimeTo", DbParameter.DbType.VarChar, 10, TimeTo);
           dbParam[23] = new DbParameter("@GSTNo", DbParameter.DbType.VarChar, 25, GSTNo.Trim());
           dbParam[24] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           dbParam[25] = new DbParameter("@ischkGstnNo", DbParameter.DbType.Bit, 1, ischkGstnNo);
           dbParam[26] = new DbParameter("@vendor", DbParameter.DbType.VarChar, 50, vendor.Trim());
           dbParam[27] = new DbParameter("@CGSTAmt", DbParameter.DbType.Decimal, 15, CGSTAmt);
           dbParam[28] = new DbParameter("@SGSTAmt", DbParameter.DbType.Decimal, 15, SGSTAmt);
           dbParam[29] = new DbParameter("@IGSTAmt", DbParameter.DbType.Decimal, 15, IGSTAmt);

           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertExpenses", dbParam);
           return Convert.ToInt32(dbParam[24].Value);
       }
       //public void InsertExpenseApproval(Int32 ExpGroupId,DataTable dtexp,int Flag)
       //{
       //    DbParameter[] dbParam = new DbParameter[3];
       //    dbParam[0] = new DbParameter("@ExpGrpId", DbParameter.DbType.Int, 10, ExpGroupId);
       //    dbParam[1] = new DbParameter("@ExApprtbl", DbParameter.DbType.Datatable, 100, dtexp);
       //    dbParam[2] = new DbParameter("@Flag", DbParameter.DbType.Int, 10, Flag);
       //    DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertExpenseApproval", dbParam);
       //}
       public void InsertExpenseClientParty(Int32 ExpGroupId,Int32 ExpDetailId, DataTable dtexp)
       {
           DbParameter[] dbParam = new DbParameter[3];
           dbParam[0] = new DbParameter("@ExpGrpId", DbParameter.DbType.Int, 10, ExpGroupId);
           dbParam[1] = new DbParameter("@ExpDetailId", DbParameter.DbType.Int, 10, ExpDetailId);
           dbParam[2] = new DbParameter("@ExClientItemGrptbl", DbParameter.DbType.Datatable, 100, dtexp);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertExpenseClientItemGroup", dbParam);
       }
       public void UpdateExpenseStatus(Int32 ExpGroupId, Int32 ExpDetailId, int IsApprRej, string PassRejRemarks, decimal ApprovedAmt, int IsSupportingApproved, int Flag, string TaxCodeApproved, Boolean IsGSTINReg, string GSTINNo, string Vendor, decimal CGSTAmt, decimal SGSTAmt, decimal IGSTAmt)
       {
           DbParameter[] dbParam = new DbParameter[14];
           dbParam[0] = new DbParameter("@ExpGrpId", DbParameter.DbType.Int, 10, ExpGroupId);
           dbParam[1] = new DbParameter("@ExpenseDetailId", DbParameter.DbType.Int, 10, ExpDetailId);
           dbParam[2] = new DbParameter("@IsApprRej", DbParameter.DbType.Int, 10, IsApprRej);
           dbParam[3] = new DbParameter("@PassRejRemarks", DbParameter.DbType.VarChar, 200, PassRejRemarks);
           dbParam[4] = new DbParameter("@ApprovedAmt", DbParameter.DbType.Int, 10, ApprovedAmt);
           dbParam[5] = new DbParameter("@IsSupportingApproved", DbParameter.DbType.Int, 10, IsSupportingApproved);
           dbParam[6] = new DbParameter("@Flag", DbParameter.DbType.Int, 10, Flag);
           dbParam[7] = new DbParameter("@TaxCodeApproved", DbParameter.DbType.VarChar, 100, TaxCodeApproved);
           dbParam[8] = new DbParameter("@IsGSTINReg", DbParameter.DbType.Bit, 1, IsGSTINReg);
           dbParam[9] = new DbParameter("@GSTINNo", DbParameter.DbType.VarChar, 15, GSTINNo);
           dbParam[10] = new DbParameter("@Vendor", DbParameter.DbType.VarChar, 200, Vendor);
           dbParam[11] = new DbParameter("@CGSTAmt", DbParameter.DbType.Decimal, 15, CGSTAmt);
           dbParam[12] = new DbParameter("@SGSTAmt", DbParameter.DbType.Decimal, 15, SGSTAmt);
           dbParam[13] = new DbParameter("@IGSTAmt", DbParameter.DbType.Decimal, 15, IGSTAmt);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "InsertupdateExpenseStatus", dbParam);
       }
       public void InsertExpenseLog(Int32 ExpGroupId, Int32 UserId, Int32 SmId, string StatusActivity, string Hostname, string IpAddress, int ActionByUserId)
       {
           DbParameter[] dbParam = new DbParameter[7];
           dbParam[0] = new DbParameter("@ExpGrpId", DbParameter.DbType.Int, 10, ExpGroupId);
           dbParam[1] = new DbParameter("@UserId", DbParameter.DbType.Int, 10, UserId);
           dbParam[2] = new DbParameter("@SmId", DbParameter.DbType.Int, 10, SmId);
           dbParam[3] = new DbParameter("@StatusActivity", DbParameter.DbType.VarChar, 10, StatusActivity);
           dbParam[4] = new DbParameter("@Hostname", DbParameter.DbType.VarChar, 25, Hostname);
           dbParam[5] = new DbParameter("@IpAddress", DbParameter.DbType.VarChar, 15, IpAddress);
           dbParam[6] = new DbParameter("@ActionByUserId", DbParameter.DbType.Int, 10, ActionByUserId);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertExpenseLogs", dbParam);
       }
       
    }
}
