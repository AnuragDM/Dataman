using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FFMS
{
    public class ImportAreas
    {
        public string InsertAreas(string mParentName, string ParentName, string AreaName, string AreaDesc, string LocationType, string Active, string SyncId, string ISD, string STD, string CityType, string ConveyanceType, string ParentType)
        {
            string result = "";
            string sqlCommand = "Sp_InsertAreas";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AreaName", AreaName);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@mParentName", mParentName);
            cmd.Parameters.AddWithValue("@ParentName", ParentName);
            cmd.Parameters.AddWithValue("@AreaDesc", AreaDesc);
            cmd.Parameters.AddWithValue("@LocationType", LocationType);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@ISD", ISD);
            cmd.Parameters.AddWithValue("@STD", STD);
            cmd.Parameters.AddWithValue("@CityType", CityType);
            cmd.Parameters.AddWithValue("@ConveyanceType", ConveyanceType);
            cmd.Parameters.AddWithValue("@ParentType", ParentType);
            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow.AddSeconds(19800));
            SqlParameter OutputParam = new SqlParameter("@OutputParam", SqlDbType.VarChar,1000);
            OutputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(OutputParam);
            //SqlParameter retParam = cmd.CreateParameter();
            //retParam.Direction = ParameterDirection.ReturnValue;
            //cmd.Parameters.Add(retParam);
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

        public int InsertAreasForm(string AreaName, string AreaType, int UnderId, string SyncId, string AreaDesc, bool isActive)
        {
            int result = 0;
            string sqlCommand = "Sp_InsertAreaForm";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AreaName", AreaName);
            cmd.Parameters.AddWithValue("@AreaType", AreaType);
            cmd.Parameters.AddWithValue("@UnderId", UnderId);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@AreaDesc", AreaDesc);
            cmd.Parameters.AddWithValue("@isActive", isActive);
            cmd.Parameters.AddWithValue("@CreatedDate", Settings.GetUTCTime());
            if (isActive)
                cmd.Parameters.AddWithValue("@BlockDate", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@BlockDate", Settings.GetUTCTime());
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

        public int UpdateAreasForm(Int64 AreaID, string AreaName, string AreaType, int UnderId, string SyncId, string AreaDesc, bool isActive)
         {
            int result = 0;
            string sqlCommand = "Sp_UpdateAreaForm";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AreaID", AreaID);
            cmd.Parameters.AddWithValue("@AreaName", AreaName);
            cmd.Parameters.AddWithValue("@AreaType", AreaType);
            cmd.Parameters.AddWithValue("@UnderId", UnderId);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@AreaDesc", AreaDesc);
            cmd.Parameters.AddWithValue("@isActive", isActive);
            cmd.Parameters.AddWithValue("@CreatedDate", Settings.GetUTCTime());
            if (isActive)
                cmd.Parameters.AddWithValue("@BlockDate", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@BlockDate", Settings.GetUTCTime());
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
         public void GentratedDisplayName(Int64 AreaID)
         {
            string sqlCommand = "sp_GentratedDisplayName";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AreaID", AreaID);
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
         public int DeleteArea(Int64 AreaID)
         {
             int result = 0;
             string sqlCommand = "Sp_DeleteMastArea";
             SqlConnection con = Connection.Instance.GetConnection();
             SqlCommand cmd = new SqlCommand(sqlCommand, con);
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.Parameters.AddWithValue("@AreaID", AreaID);
             SqlParameter retParam = cmd.CreateParameter();
             retParam.Direction = ParameterDirection.ReturnValue;
             cmd.Parameters.Add(retParam);
             try
             {
                Connection.Instance.OpenConnection(con);
                result= cmd.ExecuteNonQuery();
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