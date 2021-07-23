using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AstralFFMS.ClassFiles
{
    public class ImportWareHouse
    {
        public string UploadWareHouse(string PlantName, string PlantCode, string PlantType, string OrderType, string DivCode, string SyncId, string Active)
        {
            string result = "";
            string sqlCommand = "Sp_InsertWareHouse";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PlantName", PlantName);
            cmd.Parameters.AddWithValue("@PlantCode", PlantCode);
            if (PlantCode=="Factory")
                cmd.Parameters.AddWithValue("@PlantType", "F");
                else
            cmd.Parameters.AddWithValue("@PlantType", "W");
            cmd.Parameters.AddWithValue("@OrderType", OrderType);
            cmd.Parameters.AddWithValue("@DivCode", DivCode);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@Active", Active);
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