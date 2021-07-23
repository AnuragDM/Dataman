using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BAL.MastPage
{
    public class MastPageBAL
    {
        public int Insert(string PageName, string Module, string DisplayName, bool DisplayYN, string Img, int Parent_Id, int Level_Idx, int Idx, string Android, string Android_Form, string MenuIcon, string App)
        {
            DbParameter[] dbParam = new DbParameter[15];
            dbParam[0] = new DbParameter("@PageId", DbParameter.DbType.Int, 1, 0);
            dbParam[1] = new DbParameter("@PageName", DbParameter.DbType.VarChar, 150, PageName);
            dbParam[2] = new DbParameter("@Module", DbParameter.DbType.VarChar, 15, Module);
            dbParam[3] = new DbParameter("@DisplayName", DbParameter.DbType.VarChar, 50, DisplayName);
            dbParam[4] = new DbParameter("@DisplayYN", DbParameter.DbType.Bit, 1, DisplayYN);
            dbParam[5] = new DbParameter("@Img", DbParameter.DbType.VarChar, 100, Img);
            dbParam[6] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Insert");
            dbParam[7] = new DbParameter("@Parent_Id", DbParameter.DbType.Int, 1, Parent_Id);
            dbParam[8] = new DbParameter("@Level_Idx", DbParameter.DbType.Int, 10, Level_Idx);
            dbParam[9] = new DbParameter("@Idx", DbParameter.DbType.Int, 10, Idx);
            dbParam[10] = new DbParameter("@Android", DbParameter.DbType.VarChar, 2, Android);
            dbParam[11] = new DbParameter("@Android_Form", DbParameter.DbType.VarChar, 100, Android_Form);
            dbParam[12] = new DbParameter("@MenuIcon", DbParameter.DbType.VarChar, 500, MenuIcon);
            dbParam[13] = new DbParameter("@App", DbParameter.DbType.VarChar, 30, App);
            dbParam[14] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastPage_ups", dbParam);
            return Convert.ToInt32(dbParam[14].Value);
        }

        public int Update(string PageId, string PageName, string Module, string DisplayName, bool DisplayYN, string Img, int Parent_Id, int Level_Idx, int Idx, string Android, string Android_Form, string MenuIcon, string App)
        {
            DbParameter[] dbParam = new DbParameter[15];
            dbParam[0] = new DbParameter("@PageId", DbParameter.DbType.Int, 1, Convert.ToInt32(PageId));
            dbParam[1] = new DbParameter("@PageName", DbParameter.DbType.VarChar, 150, PageName);
            dbParam[2] = new DbParameter("@Module", DbParameter.DbType.VarChar, 15, Module);
            dbParam[3] = new DbParameter("@DisplayName", DbParameter.DbType.VarChar, 50, DisplayName);
            dbParam[4] = new DbParameter("@DisplayYN", DbParameter.DbType.Bit, 1, DisplayYN);
            dbParam[5] = new DbParameter("@Img", DbParameter.DbType.VarChar, 100, Img);
            dbParam[6] = new DbParameter("@status", DbParameter.DbType.VarChar, 15, "Update");
            dbParam[7] = new DbParameter("@Parent_Id", DbParameter.DbType.Int, 1, Parent_Id);
            dbParam[8] = new DbParameter("@Level_Idx", DbParameter.DbType.Int, 10, Level_Idx);
            dbParam[9] = new DbParameter("@Idx", DbParameter.DbType.Int, 10, Idx);
            dbParam[10] = new DbParameter("@Android", DbParameter.DbType.VarChar, 2, Android);
            dbParam[11] = new DbParameter("@Android_Form", DbParameter.DbType.VarChar, 100, Android_Form);
            dbParam[12] = new DbParameter("@MenuIcon", DbParameter.DbType.VarChar, 500, MenuIcon);
            dbParam[13] = new DbParameter("@App", DbParameter.DbType.VarChar, 30, App);
            dbParam[14] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastPage_ups", dbParam);
            return Convert.ToInt32(dbParam[14].Value);
        }


        public int delete(string PageId)
        {
            DbParameter[] dbParam = new DbParameter[2];
            dbParam[0] = new DbParameter("@PageId", DbParameter.DbType.Int, 1, Convert.ToInt32(PageId));
            dbParam[1] = new DbParameter("@ReturnVal", DbParameter.DbType.Int, 1, ParameterDirection.Output);
            DbConnectionDAL.ExecuteNonQuery(CommandType.StoredProcedure, "sp_MastPage_del", dbParam);
            return Convert.ToInt32(dbParam[1].Value);
        }
    }
}
