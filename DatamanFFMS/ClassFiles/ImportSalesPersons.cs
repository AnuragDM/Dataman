using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace FFMS
{
    public class ImportSalesPersons
    {
        public string InsertSalePersons(string ReportingPerson, string Name, string DeviceNo, string DSRAllowDays, string Active, string Address1, string Address2, string City, string Email, string Mobile, string Remarks, string EmpSyncId, string SyncId, string SalesPersonType, string ResponsibilityCentre, string DOB, string DOA, string Pincode, string EmployeeName)
        {
            string result = "";
            string sqlCommand = "Sp_InsertSalePersons";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReportingPerson", ReportingPerson);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@DeviceNo", DeviceNo);
            cmd.Parameters.AddWithValue("@DSRAllowDays", DSRAllowDays);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Address1", Address1);
            cmd.Parameters.AddWithValue("@Address2", Address2);
            cmd.Parameters.AddWithValue("@City", City);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@EmpSyncId", EmpSyncId);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@SalesPersonType", SalesPersonType);
            cmd.Parameters.AddWithValue("@ResponsibilityCentre", ResponsibilityCentre);
            
            if (!string.IsNullOrEmpty(DOB))
            cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(DOB));
            else
                cmd.Parameters.AddWithValue("@DOB", DBNull.Value);
            if (!string.IsNullOrEmpty(DOA))
                cmd.Parameters.AddWithValue("@DOA", Convert.ToDateTime(DOA));
            else
                cmd.Parameters.AddWithValue("@DOA", DBNull.Value);
            cmd.Parameters.AddWithValue("@Pincode", Pincode);
            cmd.Parameters.AddWithValue("@EmployeeName", EmployeeName);
            SqlParameter OutputParam = new SqlParameter("@OutputParam", SqlDbType.VarChar, 100);
            OutputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(OutputParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = OutputParam.Value.ToString();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }

        public int InsertSalesRepForm(string SMName, string Pin, string SalesPerType, string DeviceNo, string DSRAllowDays, bool isAdmin, string Address1, string Address2,
            string CityId, string Email, string Mobile, string Remarks, string RoleId, string UserName, string SyncId, string UnderId, int GradeId, int DeptId, int DesgId, string DOB, string DOA, string ResCenID)
        {
            int result = 0;
            string sqlCommand = "Sp_InsertSalesRepForm";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SMName", SMName);
            cmd.Parameters.AddWithValue("@SalesPerType", SalesPerType);
            cmd.Parameters.AddWithValue("@DeviceNo", DeviceNo);
            cmd.Parameters.AddWithValue("@DSRAllowDays", Convert.ToInt32(DSRAllowDays));
            cmd.Parameters.AddWithValue("@isAdmin", isAdmin);
            cmd.Parameters.AddWithValue("@Address1", Address1);
            cmd.Parameters.AddWithValue("@Address2", Address2);
            cmd.Parameters.AddWithValue("@CityId", Convert.ToInt32(CityId));
            cmd.Parameters.AddWithValue("@Pin", Convert.ToInt32(Pin));
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@RoleId", Convert.ToInt32(RoleId));
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@UnderId", UnderId);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@GradeId", GradeId);
            cmd.Parameters.AddWithValue("@DeptId", DeptId);
            cmd.Parameters.AddWithValue("@DesgId", DesgId);
            cmd.Parameters.AddWithValue("@CreatedDate", Settings.GetUTCTime());
            if (DOB != "" || DOB != string.Empty)
            { 
                cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(DOB)); 
            }
            else
            { cmd.Parameters.AddWithValue("@DOB", DBNull.Value); }
            if (DOA != "" || DOA != string.Empty)
            { 
                cmd.Parameters.AddWithValue("@DOA", Convert.ToDateTime(DOA)); 
            }
            else
            { cmd.Parameters.AddWithValue("@DOA", DBNull.Value); }
            cmd.Parameters.AddWithValue("@RescenId", Convert.ToInt32(ResCenID));
            if (isAdmin)
                cmd.Parameters.AddWithValue("@BlockDate", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@BlockDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));
            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(retParam.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }

        public string InsertSalePersonsForNavision(string ReportingPerson, string Name, string DeviceNo, string DSRAllowDays, string Active, string Address1, string Address2, string City, string Email, string Mobile, string Remarks, string EmpSyncId, string SyncId, string SalesPersonType, string ResponsibilityCentre, string DOB, string DOA, string Pincode, string EmployeeName)
        {
            string result = "";
            string sqlCommand = "[Sp_InsertSalePersonsForNavision]";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReportingPerson", ReportingPerson);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@DeviceNo", DeviceNo);
            cmd.Parameters.AddWithValue("@DSRAllowDays", DSRAllowDays);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Address1", Address1);
            cmd.Parameters.AddWithValue("@Address2", Address2);
            cmd.Parameters.AddWithValue("@City", City);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@EmpSyncId", EmpSyncId);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@SalesPersonType", SalesPersonType);
            cmd.Parameters.AddWithValue("@ResponsibilityCentre", ResponsibilityCentre);

            if (!string.IsNullOrEmpty(DOB))
                cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(DOB));
            else
                cmd.Parameters.AddWithValue("@DOB", DBNull.Value);
            if (!string.IsNullOrEmpty(DOA))
                cmd.Parameters.AddWithValue("@DOA", Convert.ToDateTime(DOA));
            else
                cmd.Parameters.AddWithValue("@DOA", DBNull.Value);
            cmd.Parameters.AddWithValue("@Pincode", Pincode);
            cmd.Parameters.AddWithValue("@EmployeeName", EmployeeName);
            SqlParameter OutputParam = new SqlParameter("@OutputParam", SqlDbType.VarChar, 100);
            OutputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(OutputParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = OutputParam.Value.ToString();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }

        public int UpdateSalesRepForm(int SMID, string SMName, string Pin, string SalesPerType, string DeviceNo, string DSRAllowDays, bool isAdmin, string Address1, string Address2, string CityId, string Email, string Mobile, string Remarks, string RoleId, string SyncId, string UnderId, int GradeId, int DeptId, int DesgId, string DOB, string DOA, string ResCenID)
        {
            int result = 0;
            string sqlCommand = "Sp_UpdateSalesRepForm";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SMID", SMID);
            cmd.Parameters.AddWithValue("@SMName", SMName);
            cmd.Parameters.AddWithValue("@SalesPerType", SalesPerType);
            cmd.Parameters.AddWithValue("@DeviceNo", DeviceNo);
            cmd.Parameters.AddWithValue("@DSRAllowDays", Convert.ToInt32(DSRAllowDays));
            cmd.Parameters.AddWithValue("@isAdmin", isAdmin);
            cmd.Parameters.AddWithValue("@Address1", Address1);
            cmd.Parameters.AddWithValue("@Address2", Address2);
            cmd.Parameters.AddWithValue("@CityId", Convert.ToInt32(CityId));
            cmd.Parameters.AddWithValue("@Pin", Convert.ToInt32(Pin));
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Mobile", Mobile);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@RoleId", Convert.ToInt32(RoleId));
            cmd.Parameters.AddWithValue("@UnderId", UnderId);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@GradeId", GradeId);
            cmd.Parameters.AddWithValue("@DeptId", DeptId);
            cmd.Parameters.AddWithValue("@DesgId", DesgId);
            cmd.Parameters.AddWithValue("@CreatedDate", Settings.GetUTCTime());
            if (DOB != "" || DOB != string.Empty)
            {
                cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(DOB));
            }
            else
            { cmd.Parameters.AddWithValue("@DOB", DBNull.Value); }
            if (DOA != "" || DOA != string.Empty)
            {
                cmd.Parameters.AddWithValue("@DOA", Convert.ToDateTime(DOA));
            }
            else
            { cmd.Parameters.AddWithValue("@DOA", DBNull.Value); }
            cmd.Parameters.AddWithValue("@RescenId", Convert.ToInt32(ResCenID));
            if (isAdmin)
                cmd.Parameters.AddWithValue("@BlockDate", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@BlockDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));

            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(retParam.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }
        public void GentratedDisplayName(Int64 SMID)
        {
            string sqlCommand = "sp_GentratedDisplayNameForSalesPerson";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SMID", SMID);
            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
        }
        public int DeleteSalesPersons(Int64 SMID)
        {
            int result = 0;
            string sqlCommand = "Sp_DeleteMastSalesRep";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SMID", SMID);
            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Instance.CloseConnection(con);
            }
            return result;
        }
    }
}