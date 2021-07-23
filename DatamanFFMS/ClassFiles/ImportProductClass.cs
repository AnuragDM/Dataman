using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FFMS.ClassFiles
{
    public class ImportProductClass
    {
        public string UploadProduct(string ProductClass, string SyncCode, string Active)
        {
            string result = "";
            string sqlCommand = "Sp_InsertProductClass";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductClass", ProductClass);
            cmd.Parameters.AddWithValue("@SyncCode", SyncCode);
            cmd.Parameters.AddWithValue("@Active", Active);    
            //SqlParameter retParam = cmd.CreateParameter();
            //retParam.Direction = ParameterDirection.ReturnValue;
            //cmd.Parameters.Add(retParam);
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