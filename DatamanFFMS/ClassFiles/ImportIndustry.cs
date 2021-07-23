using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FFMS
{
    public class ImportIndustry
    {
     
            public int InsertIndustries(DataTable dtIndustry)
            {
                int result = 0;
                string sqlCommand = "Sp_InsertIndustry";
                SqlConnection con = Connection.Instance.GetConnection();
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MastIndustryType", dtIndustry);
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
    }
}