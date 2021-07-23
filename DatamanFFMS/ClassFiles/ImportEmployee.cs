using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace AstralFFMS.ClassFiles
{
    public class ImportEmployee
    {
        public string UploadEmployee(string Name, string Password, string Active, string Email, string RoleName, string Department, string Designation, string EmpSyncId,string empName,string CostCentre)
        {
            string result = "";
            string sqlCommand = "Sp_InsertEmployee";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", Name.ToUpper());
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@RoleName", RoleName);
            cmd.Parameters.AddWithValue("@Department", Department);
            cmd.Parameters.AddWithValue("@Designation", Designation);
            cmd.Parameters.AddWithValue("@EmpSyncId", EmpSyncId);
            cmd.Parameters.AddWithValue("@EmpName", empName.ToUpper());
            cmd.Parameters.AddWithValue("@CostCentre", Convert.ToDecimal(CostCentre));
            
            SqlParameter OutputParam = new SqlParameter("@OutputParam", SqlDbType.VarChar, 100);
            OutputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(OutputParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = OutputParam.Value.ToString();
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

        public string UploadEmployeeForNavision(string Name, string Password, string Active, string Email, string RoleName, string Department, string Designation, string EmpSyncId, string empName, string CostCentre)
        {
            string result = "";
            string sqlCommand = "[Sp_InsertEmployeeForNavision]";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", Name.ToUpper());
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@RoleName", RoleName);
            cmd.Parameters.AddWithValue("@Department", Department);
            cmd.Parameters.AddWithValue("@Designation", Designation);
            cmd.Parameters.AddWithValue("@EmpSyncId", EmpSyncId);
            cmd.Parameters.AddWithValue("@EmpName", empName.ToUpper());
            cmd.Parameters.AddWithValue("@CostCentre", Convert.ToDecimal(CostCentre));

            SqlParameter OutputParam = new SqlParameter("@OutputParam", SqlDbType.VarChar, 100);
            OutputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(OutputParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                cmd.ExecuteNonQuery();
                result = OutputParam.Value.ToString();
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