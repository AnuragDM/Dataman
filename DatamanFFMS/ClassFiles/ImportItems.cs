using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FFMS
{
    public class ImportItems
    {
        public string InsertItems(string ParentName, string ItemName, string ItemCode, string Unit, string Active, string StdPack, string Syncid, decimal Mrp, decimal Dp, decimal Rp, string PriceGroup, string ProductClass, string Segment, string primaryunit, decimal primaryunitfactor, string secondaryunit, decimal secondaryunitfactor, decimal minimumqty, decimal CGSTPer, decimal SGSTPer, decimal IGSTPer, string Promoted)
        {
            string result = "";
            string sqlCommand = "Sp_InsertItems";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ParentName", ParentName);
            cmd.Parameters.AddWithValue("@ItemName", ItemName);
            cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
            cmd.Parameters.AddWithValue("@Unit", Unit);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@StdPack", StdPack);
            cmd.Parameters.AddWithValue("@Syncid", Syncid);
            cmd.Parameters.AddWithValue("@Mrp", Mrp);
            cmd.Parameters.AddWithValue("@Dp", Dp);
            cmd.Parameters.AddWithValue("@Rp", Rp);
            cmd.Parameters.AddWithValue("@PriceGroup", PriceGroup);
            cmd.Parameters.AddWithValue("@ProductClass", ProductClass);
            cmd.Parameters.AddWithValue("@Segment", Segment);
            if (primaryunit=="")
            {
                cmd.Parameters.AddWithValue("@primaryunit", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@primaryunit", primaryunit);
            }
          
            cmd.Parameters.AddWithValue("@primaryunitfactor", primaryunitfactor);
            if (secondaryunit=="")
            {
                cmd.Parameters.AddWithValue("@secondaryunit",  DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@secondaryunit", secondaryunit);
            }
            
            cmd.Parameters.AddWithValue("@secondaryunitfactor", secondaryunitfactor);
            cmd.Parameters.AddWithValue("@MOQ", minimumqty);


            cmd.Parameters.AddWithValue("@CGSTPer", CGSTPer);
            cmd.Parameters.AddWithValue("@SGSTPer", SGSTPer);
            cmd.Parameters.AddWithValue("@IGSTPer", IGSTPer);
            cmd.Parameters.AddWithValue("@Promoted", Promoted);
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
        public string InsertItemsForNavision(string ParentName, string ItemName, string ItemCode, string Active, string StdPack, string Syncid, string PriceGroup, string ProductClass, string Segment)
        {
            string result = "";
            string sqlCommand = "[Sp_InsertItemsForNavision]";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ParentName", ParentName);
            cmd.Parameters.AddWithValue("@ItemName", ItemName);
            cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
            //cmd.Parameters.AddWithValue("@Unit", Unit);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@StdPack", StdPack);
            cmd.Parameters.AddWithValue("@Syncid", Syncid);
            //cmd.Parameters.AddWithValue("@Mrp", Mrp);
            //cmd.Parameters.AddWithValue("@Dp", Dp);
            //cmd.Parameters.AddWithValue("@Rp", Rp);
            cmd.Parameters.AddWithValue("@PriceGroup", PriceGroup);
            cmd.Parameters.AddWithValue("@ProductClass", ProductClass);
            cmd.Parameters.AddWithValue("@Segment", Segment);
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
        public int InsertItemForm(string ItemName, string Unit, bool Active, string StdPack, string SyncId, decimal MRP, decimal DP, decimal RP, int UnderId, string ItemType, string ItemCode, string SegmentId, string ClassId)
        {
            int result = 0;
            string sqlCommand = "Sp_InsertItemForm";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemName", ItemName);
            if (Unit != "" || Unit != String.Empty)
            { cmd.Parameters.AddWithValue("@Unit", Unit); }
            else
            { cmd.Parameters.AddWithValue("@Unit", 0); }
            if (StdPack != "" || StdPack != String.Empty)
            { cmd.Parameters.AddWithValue("@StdPack", StdPack); }
            else
            { cmd.Parameters.AddWithValue("@StdPack", 0); }
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@MRP", MRP);
            cmd.Parameters.AddWithValue("@DP",Convert.ToDecimal(DP));
            cmd.Parameters.AddWithValue("@RP",Convert.ToDecimal( RP));
            cmd.Parameters.AddWithValue("@UnderId", UnderId);
            cmd.Parameters.AddWithValue("@ItemType", ItemType);
            cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
            cmd.Parameters.AddWithValue("@CreatedDate", Settings.GetUTCTime());
            if (SegmentId != "" || SegmentId != String.Empty)
            { cmd.Parameters.AddWithValue("@SegmentId", Convert.ToInt32(SegmentId)); }
            else
            { cmd.Parameters.AddWithValue("@SegmentId", DBNull.Value); }
            if (ClassId != "" || ClassId != String.Empty)
            { cmd.Parameters.AddWithValue("@ClassId", Convert.ToInt32(ClassId)); }
            else
            { cmd.Parameters.AddWithValue("@ClassId", DBNull.Value); }
            if (Active)
                cmd.Parameters.AddWithValue("@BlockDate", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@BlockDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));
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


        public int UpdateItemForm(int ItemId, string ItemName, string Unit, bool Active, string StdPack, string SyncId, decimal MRP, decimal DP, decimal RP, int UnderId, string ItemType, string ItemCode, string SegmentId, string ClassId)
        {
            int result = 0;
            string sqlCommand = "Sp_UpdateItemForm";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemId", ItemId);
            cmd.Parameters.AddWithValue("@ItemName", ItemName);
            if (Unit != "" || Unit != String.Empty)
            { cmd.Parameters.AddWithValue("@Unit", Unit); }
            else
            { cmd.Parameters.AddWithValue("@Unit", 0); }
            if (StdPack != "" || StdPack != String.Empty)
            { cmd.Parameters.AddWithValue("@StdPack", StdPack); }
            else
            { cmd.Parameters.AddWithValue("@StdPack", 0); }
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@SyncId", SyncId);
            cmd.Parameters.AddWithValue("@MRP", MRP);
            cmd.Parameters.AddWithValue("@DP", DP);
            cmd.Parameters.AddWithValue("@RP", RP);
            cmd.Parameters.AddWithValue("@UnderId", UnderId);
            cmd.Parameters.AddWithValue("@ItemType", ItemType);
            cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
            cmd.Parameters.AddWithValue("@CreatedDate", Settings.GetUTCTime());
            if (SegmentId != "" || SegmentId != String.Empty)
            { cmd.Parameters.AddWithValue("@SegmentId", Convert.ToInt32(SegmentId)); }
            else
            { cmd.Parameters.AddWithValue("@SegmentId", DBNull.Value); }
            if (ClassId != "" || ClassId != String.Empty)
            { cmd.Parameters.AddWithValue("@ClassId", Convert.ToInt32(ClassId)); }
            else
            { cmd.Parameters.AddWithValue("@ClassId", DBNull.Value); }
            if (Active)
                cmd.Parameters.AddWithValue("@BlockDate", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@BlockDate", DateTime.Now.ToUniversalTime().AddSeconds(19800));
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
        public int DeleteItems(Int32 ItemId)
        {
            int result = 0;
            string sqlCommand = "Sp_DeleteMastItem";
            SqlConnection con = Connection.Instance.GetConnection();
            SqlCommand cmd = new SqlCommand(sqlCommand, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemId", ItemId);
            SqlParameter retParam = cmd.CreateParameter();
            retParam.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retParam);
            try
            {
                Connection.Instance.OpenConnection(con);
                result = cmd.ExecuteNonQuery();
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