using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BAL;
using DAL;
namespace BAL.MastLink
{
   public class MastLinkBAL
    {
       public void Insert(int PrimCode, int LinkCode)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@PrimCode", DbParameter.DbType.Int, 10, PrimCode);
           dbParam[1] = new DbParameter("@LinkCode", DbParameter.DbType.Int,10, LinkCode);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertMastLink", dbParam);
       }
    }
}
