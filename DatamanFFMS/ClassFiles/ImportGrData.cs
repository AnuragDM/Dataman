using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FFMS
{
    public class ImportGrData
    {
        public string InsertGRData(string GrDocid, string Invoiceno, DateTime Invoicedate, string Transpotername, string Cartonno, string Biltyno, DateTime Biltydate, decimal Biltyamount, string DistributorSynchid)
        {
            string result = "";
            string sqlCommand = "Sp_InsertGRData";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GrDocid", GrDocid);
            cmd.Parameters.AddWithValue("@Invoiceno", Invoiceno);
            cmd.Parameters.AddWithValue("@Invoicedate", Invoicedate);
            cmd.Parameters.AddWithValue("@Transpotername", Transpotername);
            cmd.Parameters.AddWithValue("@Cartonno", Cartonno);
            cmd.Parameters.AddWithValue("@Biltyno", Biltyno);
            cmd.Parameters.AddWithValue("@Biltydate", Biltydate);
            cmd.Parameters.AddWithValue("@Biltyamount", Biltyamount);
            cmd.Parameters.AddWithValue("@DistributorSynchid", DistributorSynchid);
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
    }
}