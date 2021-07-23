using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FFMS
{
    public class ImportDistLedger
    {
        public string InsertDistLedger(string DocId, string DistributorSyncId, DateTime PostingDate, string Narration, decimal Amount, decimal crAmount, decimal DrAmount, string CompanyCode, int entryno)
        {
            string result = "";
            string sqlCommand = "Sp_InsertDistLedger";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DocId", DocId);
            cmd.Parameters.AddWithValue("@DistributorSynchId", DistributorSyncId);
            cmd.Parameters.AddWithValue("@PostingDate", PostingDate);
            cmd.Parameters.AddWithValue("@Narration", Narration);
            cmd.Parameters.AddWithValue("@Amount", Amount);
            cmd.Parameters.AddWithValue("@crAmount", crAmount);
            cmd.Parameters.AddWithValue("@DrAmount", DrAmount);
            cmd.Parameters.AddWithValue("@CompanyCode", CompanyCode);
            cmd.Parameters.AddWithValue("@entryno", entryno);            
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