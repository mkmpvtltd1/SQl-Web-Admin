using System;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;

namespace com.eforceglobal.DBAdmin.DAL
{
    internal class CommonHelper: DBHelper
    {
        internal override DbDataAdapter DataAdapter
        {
            get
            {
                switch (ProviderName)
                {
                    case "System.Data.SqlClient":
                        return new SqlDataAdapter();
                    case "MySql.Data.MySqlClient":
                        return new MySqlDataAdapter();
                    case "System.Data.OleDb":
                        return new OleDbDataAdapter();
                    default:
                        throw new Exception("Database provider not specified in connection string.");

                }
            }
        }

        internal override DbConnection Connection
        {
            get 
            {
                switch (ProviderName)
                {
                    case "System.Data.SqlClient":
                        return new SqlConnection(ConnectionString);
                    case "MySql.Data.MySqlClient":
                        return new MySqlConnection(ConnectionString); 
                    case "System.Data.OleDb":
                        return new OleDbConnection(ConnectionString);
                    default:
                        throw new Exception("Database provider not specified in connection string.");

                }
            }
        }

        internal override string SetParamPrefix(string Text)
        {

            switch (ProviderName)
            {
                case "MySql.Data.MySqlClient":
                    return Text;
                default:
                    return Text.Replace('?', '@');

            }
        }

        internal override IDbDataParameter GetParameter(DBCommandParam dbCommandParam)
        {
            IDbDataParameter param = GetDbDataParameter();
            param.Size = dbCommandParam.Size;
            param.ParameterName = SetParamPrefix(dbCommandParam.Name);
            if (ProviderName == "MySql.Data.MySqlClient"
                && dbCommandParam.DbType == DbType.Date && dbCommandParam.Value != null)
            {
                param.DbType = DbType.String;
                param.Value = FormatDateYMD(DateTime.Parse(dbCommandParam.Value.ToString()));
            }
            else
            {
                param.DbType = dbCommandParam.DbType;
                param.Value = dbCommandParam.Value;
            }
            return param;
        }

        private IDbDataParameter GetDbDataParameter()
        {
            switch (ProviderName)
            {
                case "System.Data.SqlClient":
                    return new SqlParameter();
                case "MySql.Data.MySqlClient":
                    return new MySqlParameter();
                case "System.Data.OleDb":
                    return new OleDbParameter();
                default:
                    throw new Exception("Database provider not specified in connection string.");
            }
        }
        private static string FormatDateYMD(DateTime Date)
        {
            return Date.Year + "-" + Date.Month + "-" + Date.Day;
        }
    }
}