using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BAL.TransComplaint
{
   public class TComplAll
    {
       public int Insert(string ComplNatId,string manufactDate, string distributor, string ItemId, string BatchNo, 
           string Remark, string imgURL,int UserId,DateTime Vdate,string DocID,int smID,string Atype)
        {
            DbParameter[] dbParam = new DbParameter[15];
            dbParam[0] = new DbParameter("@ComplId", DbParameter.DbType.Int, 1, 0);
            dbParam[1] = new DbParameter("@DocId", DbParameter.DbType.VarChar, 30, DocID);
            dbParam[2] = new DbParameter("@Vdate", DbParameter.DbType.DateTime, 20, Vdate);
            dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, UserId);
            dbParam[4] = new DbParameter("@ComplNatId", DbParameter.DbType.Int, 8,Convert.ToInt32(ComplNatId));
            if (ItemId!="")
            {
                dbParam[5] = new DbParameter("@ItemId", DbParameter.DbType.Int, 8, Convert.ToInt32(ItemId));
            }
            else
            {
                dbParam[5] = new DbParameter("@ItemId", DbParameter.DbType.Int, 8,DBNull.Value);
            }
            dbParam[6] = new DbParameter("@Remark", DbParameter.DbType.VarChar,8000, Remark);
            dbParam[7] = new DbParameter("@ImgUrl", DbParameter.DbType.VarChar, 255, imgURL);
            dbParam[8] = new DbParameter("@BatchNo", DbParameter.DbType.VarChar, 50, BatchNo);
            if (manufactDate!="")
            {
                dbParam[9] = new DbParameter("@ManufactureDate", DbParameter.DbType.DateTime, 8,Convert.ToDateTime(manufactDate));
            }
            else
            {
                dbParam[9] = new DbParameter("@ManufactureDate", DbParameter.DbType.DateTime, 8, DBNull.Value);
            }
            if (distributor != "")
            {
                dbParam[10] = new DbParameter("@DistId", DbParameter.DbType.Int, 1, Convert.ToInt32(distributor));
            }
            else
            {
                dbParam[10] = new DbParameter("@DistId", DbParameter.DbType.Int, 1, DBNull.Value);
            }
            dbParam[11] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            dbParam[12] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[14] = new DbParameter("@AType", DbParameter.DbType.VarChar, 1, Atype);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransComplaint_ups", dbParam);
            return Convert.ToInt32(dbParam[13].Value);
        }

       public string GetDociId(DateTime newDate)
       {
           DbParameter[] dbParam = new DbParameter[3];
           dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, "COMPL");
           dbParam[1] = new DbParameter("@V_Date", DbParameter.DbType.DateTime, 8, newDate);
           dbParam[2] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Getdocid", dbParam);
           return Convert.ToString(dbParam[2].Value);
       }

       public string SetDociId(string mDocId)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@V_Type", DbParameter.DbType.VarChar, 5, "COMPL");
           dbParam[1] = new DbParameter("@mDocId", DbParameter.DbType.VarChar, 35, mDocId);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Setdocid", dbParam);
           return Convert.ToString(1);
       }

       public int Update(Int64 ComplId, string ComplNatId, string manufactDate, string distributor, string ItemId, string BatchNo,
          string Remark, string imgURL, int UserId, DateTime Vdate, int smID, string Atype)
        {
            DbParameter[] dbParam = new DbParameter[15];
            dbParam[0] = new DbParameter("@ComplId", DbParameter.DbType.BigInt, 1, ComplId);
            dbParam[1] = new DbParameter("@DocId", DbParameter.DbType.VarChar, 30, DBNull.Value);
            dbParam[2] = new DbParameter("@Vdate", DbParameter.DbType.DateTime, 20, Vdate);
            dbParam[3] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, UserId);
            dbParam[4] = new DbParameter("@ComplNatId", DbParameter.DbType.Int, 8, ComplNatId);
            if (ItemId != "")
            {
                dbParam[5] = new DbParameter("@ItemId", DbParameter.DbType.Int, 8, ItemId);
            }
            else
            {
                dbParam[5] = new DbParameter("@ItemId", DbParameter.DbType.Int, 8, DBNull.Value);
            }
            dbParam[6] = new DbParameter("@Remark", DbParameter.DbType.VarChar, 8000, Remark);
            dbParam[7] = new DbParameter("@ImgUrl", DbParameter.DbType.VarChar, 255, imgURL);
            dbParam[8] = new DbParameter("@BatchNo", DbParameter.DbType.VarChar, 50, BatchNo);
            if (manufactDate != "")
            {
                dbParam[9] = new DbParameter("@ManufactureDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(manufactDate));
            }
            else
            {
                dbParam[9] = new DbParameter("@ManufactureDate", DbParameter.DbType.DateTime, 8, DBNull.Value);
            }
            if (distributor != "")
            {
                dbParam[10] = new DbParameter("@DistId", DbParameter.DbType.Int, 1, Convert.ToInt32(distributor));
            }
            else
            {
                dbParam[10] = new DbParameter("@DistId", DbParameter.DbType.Int, 1, DBNull.Value);
            }
            dbParam[11] = new DbParameter("@SMId", DbParameter.DbType.Int, 1, smID);
            dbParam[12] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[14] = new DbParameter("@AType", DbParameter.DbType.VarChar, 1, Atype);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransComplaint_ups", dbParam);
            return Convert.ToInt32(dbParam[13].Value);
        }
       public int delete(string ComplId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@ComplId", DbParameter.DbType.Int, 1, Convert.ToInt32(ComplId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_TransComplaint_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

       public int InsertResponse(Int64 ComplainId, string DocID, Int32 UserId, string Remarks, Boolean Status)
       {
           DbParameter[] dbParam = new DbParameter[6];
           dbParam[0] = new DbParameter("@ComplainId", DbParameter.DbType.BigInt, 1, Convert.ToInt64(ComplainId));
           dbParam[1] = new DbParameter("@DocID", DbParameter.DbType.VarChar, 50, DocID);
           dbParam[2] = new DbParameter("@UserId", DbParameter.DbType.Int, 1, Convert.ToInt32(UserId));
           dbParam[3] = new DbParameter("@Remarks", DbParameter.DbType.VarChar, 2000,Remarks);
           dbParam[4] = new DbParameter("@Status", DbParameter.DbType.Bit, 1, Status);
           dbParam[5] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertComplainRespond", dbParam);
           return Convert.ToInt32(dbParam[5].Value);
       }    
    }
}
