using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;

namespace BAL.Uploads
{
  public  class UploadBAL
    {
      public int Insert(string DocDate, string DocID, string Title, string LinkURL, bool Active, string DocFor, int SMID, int UserID, string smids, string distid)
        {
            DbParameter[] dbParam = new DbParameter[13];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, 0);
            dbParam[1] = new DbParameter("@DocID", DbParameter.DbType.VarChar, 30, DocID);
            dbParam[2] = new DbParameter("@UserID", DbParameter.DbType.Int, 4, UserID);
            if (DocDate == "")
            {
                dbParam[3] = new DbParameter("@DocDate", DbParameter.DbType.DateTime, 8, SqlDateTime.Null);
            }
            else
            {
                dbParam[3] = new DbParameter("@DocDate", DbParameter.DbType.DateTime, 8, Convert.ToDateTime(DocDate));
            }
            dbParam[4] = new DbParameter("@SMID", DbParameter.DbType.Int, 4, SMID);
            dbParam[5] = new DbParameter("@DocFor", DbParameter.DbType.VarChar, 50, DocFor);
            dbParam[6] = new DbParameter("@Active", DbParameter.DbType.Bit,1, Active);
            dbParam[7] = new DbParameter("@Title", DbParameter.DbType.VarChar,50, Title);
            dbParam[8] = new DbParameter("@LinkURL", DbParameter.DbType.VarChar, 900, LinkURL);
            dbParam[9] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[10] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[11] = new DbParameter("@smids", DbParameter.DbType.VarChar, 8000, smids);
            dbParam[12] = new DbParameter("@distid", DbParameter.DbType.VarChar, 8000, distid);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "udp_UploadDocuments_ups", dbParam);
            return Convert.ToInt32(dbParam[10].Value);
        }       

        public int delete(string VisId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@id", DbParameter.DbType.Int, 1, Convert.ToInt32(VisId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "udp_UploadDocuments_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

    }
}
