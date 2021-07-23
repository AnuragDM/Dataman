using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;
namespace BAL
{
   public class CRMBAL
    {
       public int InsertUpdateStatus(int StatusId, string status, string description)
       {
           DbParameter[] dbParam = new DbParameter[4];
           dbParam[0] = new DbParameter("@Status_Id", DbParameter.DbType.Int, 1, StatusId);
           dbParam[1] = new DbParameter("@Status", DbParameter.DbType.VarChar, 50,status );
           dbParam[2] = new DbParameter("@description", DbParameter.DbType.VarChar, 150, description);
           dbParam[3] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastCRMStatus", dbParam);
           return Convert.ToInt32(dbParam[3].Value);
       }
       public int deleteStatus(string Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteCRMStatus", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }

       public int InsertUpdateTag(int TagId, string Tag, string description)
       {
           DbParameter[] dbParam = new DbParameter[4];
           dbParam[0] = new DbParameter("@Tag_Id", DbParameter.DbType.Int, 1, TagId);
           dbParam[1] = new DbParameter("@Tag", DbParameter.DbType.VarChar, 50,Tag );
           dbParam[2] = new DbParameter("@description", DbParameter.DbType.VarChar, 150, description);
           dbParam[3] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_MastCRMTag", dbParam);
           return Convert.ToInt32(dbParam[3].Value);
       }
       public int InsertMastContacsType(int id,string value, string data,int sort,string flag)
       {

           DbParameter[] dbParam = new DbParameter[6];
           dbParam[0] = new DbParameter("@id", DbParameter.DbType.Int, 1, id);
           dbParam[1] = new DbParameter("@value", DbParameter.DbType.VarChar, 30, value);
           dbParam[2] = new DbParameter("@data", DbParameter.DbType.VarChar, 30, data);
           dbParam[3] = new DbParameter("@sort", DbParameter.DbType.Int,1, sort);
           dbParam[4] = new DbParameter("@flag", DbParameter.DbType.VarChar,1, flag);
           dbParam[5] = new DbParameter("@ReturnVal", DbParameter.DbType.Int,1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_MastContacsType", dbParam);
           return Convert.ToInt32(dbParam[5].Value);
       }
       public int InsertMastleadcompany(int id, string CompName, string CompDesc, string CompPhone, string CompUrl, string CompAdd, string city, string State, int zip, int countryid,  string flag)
       {
           DbParameter[] dbParam = new DbParameter[12];
           dbParam[0] = new DbParameter("@id", DbParameter.DbType.Int, 1, id);
           dbParam[1] = new DbParameter("@CompName", DbParameter.DbType.VarChar, 50, CompName);
           dbParam[2] = new DbParameter("@CompDesc", DbParameter.DbType.VarChar, 300, CompDesc);
           dbParam[3] = new DbParameter("@CompPhone", DbParameter.DbType.VarChar, 13, CompPhone);
           dbParam[4] = new DbParameter("@CompUrl", DbParameter.DbType.VarChar, 50, CompUrl);
           dbParam[5] = new DbParameter("@CompAdd", DbParameter.DbType.VarChar, 250, CompAdd);
           dbParam[6] = new DbParameter("@city", DbParameter.DbType.VarChar, 30, city);
           dbParam[7] = new DbParameter("@State", DbParameter.DbType.VarChar, 30, State);
           dbParam[8] = new DbParameter("@zip", DbParameter.DbType.Int, 6, zip);
           dbParam[9] = new DbParameter("@countryid", DbParameter.DbType.Int, 5, countryid);
           dbParam[10] = new DbParameter("@flag", DbParameter.DbType.VarChar,1, flag);
           dbParam[11] = new DbParameter("@ReturnVal", DbParameter.DbType.Int,1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_MastLeadCompany", dbParam);
           return Convert.ToInt32(dbParam[11].Value);
       }
       public int deleteTag(string Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteCRMTag", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
       public int InsertUpdateLead(int LeadId, string Lead)
       {
           DbParameter[] dbParam = new DbParameter[3];
           dbParam[0] = new DbParameter("@Lead_Id", DbParameter.DbType.Int, 1, LeadId);
           dbParam[1] = new DbParameter("@Lead", DbParameter.DbType.VarChar, 50, Lead);
           dbParam[2] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_MastCRMLead", dbParam);
           return Convert.ToInt32(dbParam[2].Value);
       }
       public int deleteLead(string Id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, Convert.ToInt32(Id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteCRMLead", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }
       public int deleteLeadContact(string Contact_id)
       {
           DbParameter[] dbParam = new DbParameter[2];
           dbParam[0] = new DbParameter("@Id", DbParameter.DbType.Int, 1, Convert.ToInt32(Contact_id));
           dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_DeleteCRMLeadContact", dbParam);
           return Convert.ToInt32(dbParam[1].Value);
       }

       public int InsertUpdateContact(int ContactId, string  FirstName,string LastName,string JobTitle,string Company,string Address,string City,string State,string Country,string ZipCode,string Status_Id,string Tag_Id,string Lead_Id, string Background,string Customfields)
       {
           DbParameter[] dbParam = new DbParameter[16];
           dbParam[0] = new DbParameter("@ContactId", DbParameter.DbType.Int, 1, ContactId);
           dbParam[1] = new DbParameter("@FirstName", DbParameter.DbType.VarChar, 20, FirstName);
           dbParam[2] = new DbParameter("@LastName", DbParameter.DbType.VarChar, 20, LastName);
           dbParam[3] = new DbParameter("@JobTitle", DbParameter.DbType.VarChar, 50, JobTitle);
           dbParam[4] = new DbParameter("@Address", DbParameter.DbType.VarChar, 150, Address);
           dbParam[5] = new DbParameter("@City", DbParameter.DbType.VarChar, 50, City);
           dbParam[6] = new DbParameter("@State", DbParameter.DbType.VarChar, 50, State);
           dbParam[7] = new DbParameter("@Country", DbParameter.DbType.VarChar, 50, Country);
           dbParam[8] = new DbParameter("@ZipCode", DbParameter.DbType.VarChar, 6, ZipCode);
           dbParam[9] = new DbParameter("@Status_Id", DbParameter.DbType.Int, 1, Status_Id);
           dbParam[10] = new DbParameter("@Tag_Id", DbParameter.DbType.Int, 1, Tag_Id);
           dbParam[11] = new DbParameter("@Lead_Id", DbParameter.DbType.Int, 1, Lead_Id);
           dbParam[12] = new DbParameter("@Background", DbParameter.DbType.VarChar, 150, Background);
           dbParam[13] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
           dbParam[14] = new DbParameter("@Company", DbParameter.DbType.VarChar, 50, Company);
           dbParam[15] = new DbParameter("@Customfields", DbParameter.DbType.VarChar, 50, Customfields);
           DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "Sp_InsertCRMContact", dbParam);
           return Convert.ToInt32(dbParam[13].Value);
       }
    }


    
}
