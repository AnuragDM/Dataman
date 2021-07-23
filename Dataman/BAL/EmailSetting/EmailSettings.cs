using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BAL.EmailSetting
{
    public class EmailSettings
    {
        public int Insert(string SenderEmailID, string Password, string Port, string PurchaseEmailID, string PurchaseEmailCC,
            string OrderApprovalEmailId, string OrderApprovalEmailCC, string ChangePasswordEmailId, string ChangePasswordEmailCC, string MailServer, string ExpenseAdminEmailID)
        {
            DbParameter[] dbParam = new DbParameter[14];
            dbParam[0] = new DbParameter("@SenderEmailId", DbParameter.DbType.VarChar, 50, SenderEmailID);
            dbParam[1] = new DbParameter("@SenderPassword", DbParameter.DbType.VarChar, 50, Password);
            dbParam[2] = new DbParameter("@Port", DbParameter.DbType.Int, 4, Port);
            dbParam[3] = new DbParameter("@PurchaseEmailId", DbParameter.DbType.VarChar, 2000, PurchaseEmailID);
            dbParam[4] = new DbParameter("@PurchaseEmailCC", DbParameter.DbType.VarChar, 2000, PurchaseEmailCC);
            dbParam[5] = new DbParameter("@ChangePasswordEmailId", DbParameter.DbType.VarChar, 1000, ChangePasswordEmailId);
            dbParam[6] = new DbParameter("@ChangePasswordCC", DbParameter.DbType.VarChar, 1000, ChangePasswordEmailCC);
            dbParam[7] = new DbParameter("@ForgetPasswordEmailId", DbParameter.DbType.VarChar, 1000, ChangePasswordEmailId);
            dbParam[8] = new DbParameter("@ForgetPasswordCC", DbParameter.DbType.VarChar, 1000, ChangePasswordEmailCC);
            dbParam[9] = new DbParameter("@OrderListEmailId", DbParameter.DbType.VarChar, 1000, OrderApprovalEmailId);
            dbParam[10] = new DbParameter("@OrderListEmailCC", DbParameter.DbType.VarChar, 1000, OrderApprovalEmailCC);
            dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            dbParam[12] = new DbParameter("@MailServer", DbParameter.DbType.VarChar, 100, MailServer);
            dbParam[13] = new DbParameter("@ExpenseAdminEmailID", DbParameter.DbType.VarChar, 1000, ExpenseAdminEmailID);

            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UpdateEmailSettings", dbParam);
            return Convert.ToInt32(dbParam[11].Value);
        }
        public int InsertEnviro(string distributorsearch, string Itemsearch, string itemwisesecondary, string Areawisedistributor)
        {
            DbParameter[] dbParam = new DbParameter[5];
            dbParam[0] = new DbParameter("@distributorsearch", DbParameter.DbType.VarChar, 1, distributorsearch);
            dbParam[1] = new DbParameter("@Itemsearch", DbParameter.DbType.VarChar, 1, Itemsearch);
            dbParam[2] = new DbParameter("@itemwisesecondary", DbParameter.DbType.VarChar, 1, itemwisesecondary);
            dbParam[3] = new DbParameter("@Areawisedistributor", DbParameter.DbType.VarChar, 1, Areawisedistributor);
            dbParam[4] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);


            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UpdateEnviroSettings", dbParam);
            return Convert.ToInt32(dbParam[4].Value);
        }
    }
}
