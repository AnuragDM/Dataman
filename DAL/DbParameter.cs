namespace DAL
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public sealed class DbParameter
    {
        public ParameterDirection ParamDirection;
        public string ParameterName;
        public int ParameterSize;
        public DbType ParameterType;
        public object Value;

        public DbParameter()
        {
            this.ParamDirection = ParameterDirection.Input;
            this.ParameterSize = 0;
        }

        public DbParameter(string strParamName, DbType dtpParam, int intParamSize, ParameterDirection pdParam)
        {
            this.ParamDirection = ParameterDirection.Input;
            this.ParameterSize = 0;
            this.ParameterName = strParamName;
            this.ParameterType = dtpParam;
            this.ParameterSize = intParamSize;
            this.ParamDirection = pdParam;
        }

        public DbParameter(string strParamName, DbType dtpParam, int intParamSize, object objValue)
        {
            this.ParamDirection = ParameterDirection.Input;
            this.ParameterSize = 0;
            this.ParameterName = strParamName;
            this.Value = objValue;
            this.ParameterType = dtpParam;
            this.ParameterSize = intParamSize;
        }

        public DbParameter(string strParamName, DbType dtpParam, int intParamSize, object objValue, ParameterDirection pdParam)
        {
            this.ParamDirection = ParameterDirection.Input;
            this.ParameterSize = 0;
            this.ParameterName = strParamName;
            this.Value = objValue;
            this.ParameterType = dtpParam;
            this.ParameterSize = intParamSize;
            this.ParamDirection = pdParam;
        }

        private static SqlDbType GetSqlDbType(DbType dtpParam)
        {
            SqlDbType stpParam = SqlDbType.VarChar;
            switch (dtpParam)
            {
                case DbType.Bit:
                    return SqlDbType.Bit;

                case DbType.Char:
                    return SqlDbType.Char;

                case DbType.DateTime:
                    return SqlDbType.DateTime;

                case DbType.Decimal:
                    return SqlDbType.Decimal;

                case DbType.Float:
                    return SqlDbType.Float;

                case DbType.Int:
                    return SqlDbType.Int;

                case DbType.Money:
                    return SqlDbType.Money;

                case DbType.NText:
                    return SqlDbType.NText;

                case DbType.NVarChar:
                    return SqlDbType.NVarChar;

                case DbType.Real:
                    return SqlDbType.Real;

                case DbType.SmallInt:
                    return SqlDbType.SmallInt;

                case DbType.Text:
                    return SqlDbType.Text;

                case DbType.TinyInt:
                    return SqlDbType.TinyInt;

                case DbType.VarChar:
                    return SqlDbType.VarChar;

                case DbType.Xml:
                    return SqlDbType.Xml;
                case DbType.BigInt:
                    return SqlDbType.BigInt;
                case DbType.Datatable:
                    return SqlDbType.Structured;
            }
            return stpParam;
        }

        public static SqlParameter[] GetSqlParameter(DbParameter[] dbParam)
        {
            SqlParameter[] spParam = new SqlParameter[dbParam.Length];
            for (int intIncr = 0; intIncr < dbParam.Length; intIncr++)
            {
                spParam[intIncr] = new SqlParameter(dbParam[intIncr].ParameterName, GetSqlDbType(dbParam[intIncr].ParameterType), dbParam[intIncr].ParameterSize);
                spParam[intIncr].Direction = dbParam[intIncr].ParamDirection;
                spParam[intIncr].Value = dbParam[intIncr].Value;
            }
            return spParam;
        }

        public static void SetDbParameterValueFromSql(SqlParameter[] spParam, DbParameter[] dbParam)
        {
            for (int intIncr = 0; intIncr < spParam.Length; intIncr++)
            {
                dbParam[intIncr].Value = spParam[intIncr].Value;
            }
        }

        public enum DbType
        {
            Bit,
            Char,
            DateTime,
            Decimal,
            Float,
            Int,
            Money,
            NText,
            NVarChar,
            Real,
            SmallInt,
            Text,
            TinyInt,
            VarChar,
            Xml,
            BigInt,
            Datatable
        }
    }
}

