using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace BusinessLayer
{
    public class Connection
    {
        static Connection objConnection;
        static readonly object padlock = new object();
        static SqlConnection objSqlConnection;
        public static Connection Instance
        {
            get
            {
                return GetInstance();
            }
        }
        private static Connection GetInstance()
        {
            lock (padlock)
            {
                //if (objConnection == null)
                objConnection = new Connection();
            }
            return objConnection;
        }
        public SqlConnection GetConnection()
        {
            lock (padlock)
            {
                //if (objSqlConnection == null)
                string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                objSqlConnection = new SqlConnection( constr);
            }
            return objSqlConnection;
        }
        public void OpenConnection(SqlConnection obj)
        {
            if (obj.State != ConnectionState.Open)
                obj.Open();
        }
        public void CloseConnection(SqlConnection obj)
        {
            if (obj.State != ConnectionState.Closed)
                obj.Close();
        }
    }
}
