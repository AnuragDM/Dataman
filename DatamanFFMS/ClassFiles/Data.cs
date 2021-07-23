using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace FFMS
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
        #endregion


        public void UploadFile(FileUpload UploadControl, string FileName, string Path)
        {
            UploadControl.SaveAs(HttpContext.Current.Server.MapPath(Path) + FileName);
        }
        public DataTable ReadXLS(string FilePath, string FileName, string Sheet, bool IsHeader)
        {
            string HDR = IsHeader ? "Yes" : "No";
            DataTable dt = new DataTable();
            //string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath(FilePath + "\\" + FileName) +
            //          @";Extended Properties=""Excel 8.0;HDR=" + HDR;
            //conStr += @";IMEX=1\""";

            string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + HttpContext.Current.Server.MapPath(FilePath + "\\" + FileName) +
                  @";Extended Properties=""Excel 8.0;HDR=" + HDR;
            conStr += @";IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text""";
            OleDbConnection con = new OleDbConnection(conStr);
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [" + Sheet + "]", con);
            da.Fill(dt);
            return dt;
        }

        //public DataTable ReadXLSWithDataTypeCols(string FilePath, string FileName, string Sheet, bool IsHeader)
        //{
        //    string HDR = IsHeader ? "Yes" : "No";
        //    DataTable dt = new DataTable();
        //    string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath(FilePath + "\\" + FileName) +
        //              @";Extended Properties=""Excel 8.0;HDR=" + HDR;
        //    conStr += @";IMEX=1\""";
        //    OleDbConnection con = new OleDbConnection(conStr);
        //    OleDbDataAdapter da = new OleDbDataAdapter("select * from [" + Sheet + "]", con);
        //    da.Fill(dt);
        //    dt.Rows.RemoveAt(0);
        //    dt.AcceptChanges();
          
        //    return dt;
        //}
        public DataTable ReadXLSWithDataTypeCols(string FilePath, string FileName, string Sheet, bool IsHeader)
        {
            string HDR = IsHeader ? "Yes" : "No";
            DataTable dt = new DataTable();
            //string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath(FilePath + "\\" + FileName) +
            //          @";Extended Properties=""Excel 8.0;HDR=" + HDR;
            //conStr += @";IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text""";

            string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + HttpContext.Current.Server.MapPath(FilePath + "\\" + FileName) +
                     @";Extended Properties=""Excel 8.0;HDR=" + HDR;
            conStr += @";IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text""";
            OleDbConnection con = new OleDbConnection(conStr);
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [" + Sheet + "]", con);
            da.Fill(dt);
            dt.Rows.RemoveAt(0);
            dt.AcceptChanges();

            return dt;
        }
    }
}