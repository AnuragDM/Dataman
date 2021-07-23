using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FFMS.ClassFiles
{
    public class ImportMaterialGroup
    {
        public string InsertMaterialGrp( string ItemName, string ItemType, string Active, string Syncid)
        {
            string result = "";
            string sqlCommand = "Sp_InsertMaterialGroup";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            
            cmd.Parameters.AddWithValue("@ItemName", ItemName);
            cmd.Parameters.AddWithValue("@ItemType", ItemType);  
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Syncid", Syncid);

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