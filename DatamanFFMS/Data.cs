using BusinessLayer;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AstralFFMS
{
    public class Data
    {
        #region Insert Update Permissions
        public int InsertUpdatePermission(Int32 roleid, Int32 pageid, bool view, bool add, bool edit, bool delete, bool export)
        {
            int result = 0;


            string sqlCommand = "sp_insertupdatePermissions";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Role_Id", roleid);
            cmd.Parameters.AddWithValue("@Page_Id", pageid);
            cmd.Parameters.AddWithValue("@ViewP", view);
            cmd.Parameters.AddWithValue("@AddP", add);
            cmd.Parameters.AddWithValue("@EditP", edit);
            cmd.Parameters.AddWithValue("@DeleteP", delete);
            cmd.Parameters.AddWithValue("@ExportP", export);
          
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

        public int InsertUpdatePermissionAndroid(Int32 roleid, Int32 pageid, bool view, bool add, bool edit, bool delete, bool export,string app)
        {
            int result = 0;


            string sqlCommand = "sp_insertupdatePermissions_Android";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Role_Id", roleid);
            cmd.Parameters.AddWithValue("@Page_Id", pageid);
            cmd.Parameters.AddWithValue("@ViewP", view);
            cmd.Parameters.AddWithValue("@AddP", add);
            cmd.Parameters.AddWithValue("@EditP", edit);
            cmd.Parameters.AddWithValue("@DeleteP", delete);
            cmd.Parameters.AddWithValue("@ExportP", export);
            cmd.Parameters.AddWithValue("@app", app);

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

        public int InsertUpdatePermissionActivity(Int32 roleid, Int32 activityId, string activityName, bool allow)
        {
            int result = 0;


            string sqlCommand = "sp_insertupdatePermissionsCRM";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Role_Id", roleid);
            cmd.Parameters.AddWithValue("@Activity_Id", activityId);
            cmd.Parameters.AddWithValue("@ActivityName", activityName);
            cmd.Parameters.AddWithValue("@Allow", allow);


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
        #endregion
    }
}