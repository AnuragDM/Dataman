using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FFMS
{
    public class ImportPriceList
    {
        public string InsertUpdatePriceList(DateTime WEFDate, string ItemSyncId, decimal MRP, decimal DP, decimal RP)
        {
            string result = "";
            string sqlCommand = "Sp_InsertPriceList";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@WEFDate", WEFDate);
            cmd.Parameters.AddWithValue("@ItemSyncId", ItemSyncId);
            cmd.Parameters.AddWithValue("@MRP", MRP);
            cmd.Parameters.AddWithValue("@DP", DP);
            cmd.Parameters.AddWithValue("@RP", RP);
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

        public string InsertUpdatePriceList_V_1_0(DateTime WEFDate, string ItemSyncId, decimal MRP, decimal DP, decimal RP, string PriceListApplicability, string Country_State_City_Dist_id)
        {
            string result = "";
            string sqlCommand = "Sp_InsertPriceListNew";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@WEFDate", WEFDate);
            cmd.Parameters.AddWithValue("@ItemSyncId", ItemSyncId);
            cmd.Parameters.AddWithValue("@MRP", MRP);
            cmd.Parameters.AddWithValue("@DP", DP);
            cmd.Parameters.AddWithValue("@RP", RP);
            cmd.Parameters.AddWithValue("@PriceListApplicability", PriceListApplicability);
            cmd.Parameters.AddWithValue("@Country_State_City_Dist_id", Country_State_City_Dist_id);          
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