using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Data.SqlTypes;

namespace BAL.EmailTemplate
{
  public  class EmailTemplateBAL
    {
        public int Insert(string  EmialKey,string  Subject,string TemplateValue)
        {
            DbParameter[] dbParam = new DbParameter[6];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, 0);
            dbParam[1] = new DbParameter("@EmialKey", DbParameter.DbType.VarChar,50, EmialKey);
            dbParam[2] = new DbParameter("@Subject", DbParameter.DbType.VarChar, 250, Subject);
            dbParam[3] = new DbParameter("@TemplateValue", DbParameter.DbType.VarChar,4000, TemplateValue);
            dbParam[4] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[5] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_EmailTemplate_ups", dbParam);
            return Convert.ToInt32(dbParam[5].Value);
        }
        public int Update(int Id, string EmialKey, string Subject, string TemplateValue)
        {
            DbParameter[] dbParam = new DbParameter[6];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 4, Id);
            dbParam[1] = new DbParameter("@EmialKey", DbParameter.DbType.VarChar, 50, EmialKey);
            dbParam[2] = new DbParameter("@Subject", DbParameter.DbType.VarChar, 250, Subject);
            dbParam[3] = new DbParameter("@TemplateValue", DbParameter.DbType.VarChar, 4000, TemplateValue);
            dbParam[4] = new DbParameter("@Status", DbParameter.DbType.VarChar, 15, "update");
            dbParam[5] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_EmailTemplate_ups", dbParam);
            return Convert.ToInt32(dbParam[5].Value);
        }

        public int delete(string VisId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, Convert.ToInt32(VisId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_EmailTemplate_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }

    }
}
