using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Web;
using com.eforceglobal.DBAdmin.Constants;
using com.eforceglobal.DBAdmin.Utils;

namespace com.eforceglobal.DBAdmin.DAL
{
    public abstract class DBHelper
    {
        #region Private Fields

        private DbConnection _connectionWithTransaction = null;
        private DbCommand _commandWithTransaction = null;
        string connectionString = null;
        string providerName = null;
        private bool transactionOn = false;
        private int? _commandTimeout;
        #endregion

        #region Protected Members

        protected string ConnectionString
        {
            get
            {
                // make sure conection string is not empty
                if (connectionString == string.Empty || connectionString.Length == 0)
                    throw new ArgumentException("Invalid database connection string.");
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        protected string ProviderName
        {
            get
            {
                //as of now, we are dealing with sql server only
                providerName = "System.Data.SqlClient";
                return providerName;
            }
            set
            {
                providerName = value;
            }
        }

        #endregion

        #region Public Properties
        public int? CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;
            }
        }
        public string UserQueryInput
        {
            get;
            set;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        public void BeginTransaction()
        {
            transactionOn = true;
        }

        /// <summary>
        /// Commits the transaction
        /// </summary>
        public void CommitTransaction()
        {
            _commandWithTransaction.Transaction.Commit();
            _connectionWithTransaction.Close();
            transactionOn = false;
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (_commandWithTransaction != null)
            {
                if (_connectionWithTransaction != null)
                {
                    if (_connectionWithTransaction.State == ConnectionState.Open)
                    {
                        _commandWithTransaction.Transaction.Rollback();
                        _connectionWithTransaction.Close();
                        transactionOn = false;
                    }
                }
            }
        }

        /// <summary>
        /// Adds or refreshes rows in a DataTable within the DataSet using the specified SQL SELECT statement retrieved / generated
        /// with the help of the CommandKey and Param. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Param"></param>
        /// <param name="Dataset"></param>
        public void FillDataSet<T>(string CommandKey, string DatabaseName, DBCommandParam Param, T Dataset) where T : DataSet
        {
            FillDS<T>(GetDbCommand(CommandKey, Param, DatabaseName), Dataset, null);
        }

        /// <summary>
        /// Adds or refreshes rows in a DataTable within the DataSet using the specified SQL SELECT statement retrieved / generated
        /// with the help of the CommandKey and Params. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Params"></param>
        /// <param name="Dataset"></param>
        public void FillDataSet<T>(string CommandKey, string DatabaseName, List<DBCommandParam> Params, T Dataset) where T : DataSet
        {
            FillDS<T>(GetDbCommand(CommandKey, Params, DatabaseName), Dataset, null);
        }

        /// <summary>
        /// Adds or refreshes rows in a DataTable within the DataSet using the specified SQL SELECT statement retrieved / generated
        /// with the help of the CommandKey. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Dataset"></param>
        public void FillDataSet<T>(string CommandKey, string DatabaseName, T Dataset) where T : DataSet
        {
            FillDS<T>(GetDbCommand(CommandKey, DatabaseName), Dataset, null);
        }

        /// <summary>
        /// Adds or refreshes rows in a DataTable, name of which is provided, using the specified SQL SELECT statement retrieved / generated
        /// with the help of the CommandKey and Param. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Param"></param>
        /// <param name="Dataset"></param>
        /// <param name="TableName"></param>
        public void FillDataSet<T>(string CommandKey, string DatabaseName, DBCommandParam Param, T Dataset, string TableName) where T : DataSet
        {
            FillDS<T>(GetDbCommand(CommandKey, Param, DatabaseName), Dataset, TableName);
        }

        /// <summary>
        /// Adds or refreshes rows in a DataTable, name of which is provided, using the specified SQL SELECT statement retrieved / generated
        /// with the help of the CommandKey and Params. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Params"></param>
        /// <param name="Dataset"></param>
        /// <param name="TableName"></param>
        public void FillDataSet<T>(string CommandKey, string DatabaseName, List<DBCommandParam> Params, T Dataset, string TableName) where T : DataSet
        {
            FillDS<T>(GetDbCommand(CommandKey, Params, DatabaseName), Dataset, TableName);
        }

        /// <summary>
        /// Adds or refreshes rows in a DataTable, name of which is provided, using the specified SQL SELECT statement retrieved / generated
        /// with the help of the CommandKey. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Dataset"></param>
        /// <param name="TableName"></param>
        public void FillDataSet<T>(string CommandKey, string DatabaseName, T Dataset, string TableName) where T : DataSet
        {
            FillDS<T>(GetDbCommand(CommandKey, DatabaseName), Dataset, TableName);
        }

        /// <summary>
        /// Adds or refreshes rows in a DataTable using the specified SQL SELECT statement retrieved / generated
        /// with the help of the CommandKey and Param.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Param"></param>
        /// <param name="Datatable"></param>
        public void FillDataTable<T>(string CommandKey, string DatabaseName, DBCommandParam Param, T Datatable) where T : DataTable
        {
            FillDT<T>(GetDbCommand(CommandKey, Param, DatabaseName), Datatable);
        }

        /// <summary>
        /// Adds or refreshes rows in a DataTable using the specified SQL SELECT statement retrieved / generated
        /// with the help of the CommandKey and Params.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Params"></param>
        /// <param name="Datatable"></param>
        public void FillDataTable<T>(string CommandKey, string DatabaseName, List<DBCommandParam> Params, T Datatable) where T : DataTable
        {
            FillDT<T>(GetDbCommand(CommandKey, Params, DatabaseName), Datatable);
        }

        /// <summary>
        /// Adds or refreshes rows in a DataTable using the specified SQL SELECT statement retrieved / generated
        /// with the help of the CommandKey.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Datatable"></param>
        public void FillDataTable<T>(string CommandKey, string DatabaseName, T Datatable) where T : DataTable
        {
            FillDT<T>(GetDbCommand(CommandKey, DatabaseName), Datatable);
        }

        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Params"></param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(string CommandKey, string DatabaseName, List<DBCommandParam> Params)
        {
            return ExecuteNQuery(GetDbCommand(CommandKey, Params, DatabaseName));
        }

        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Param"></param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(string CommandKey, string DatabaseName, DBCommandParam Param)
        {
            return ExecuteNQuery(GetDbCommand(CommandKey, Param, DatabaseName));
        }

        /// <summary>
        /// Executes a SQL statement against a connection object.
        /// </summary>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(string CommandKey, string DatabaseName)
        {
            return ExecuteNQuery(GetDbCommand(CommandKey, DatabaseName));
        }

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result
        /// set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Params"></param>
        /// <returns>The first column of the first row in the result set.</returns>
        public object ExecuteScalar(string CommandKey, string DatabaseName, List<DBCommandParam> Params)
        {
            return ExecuteScalarCommand(GetDbCommand(CommandKey, Params, DatabaseName));
        }

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result
        /// set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <param name="Param"></param>
        /// <returns>The first column of the first row in the result set.</returns>
        public object ExecuteScalar(string CommandKey, string DatabaseName, DBCommandParam Param)
        {
            return ExecuteScalarCommand(GetDbCommand(CommandKey, Param, DatabaseName));
        }

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result
        /// set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <param name="CommandKey"></param>
        /// <param name="DatabaseName"></param>
        /// <returns>The first column of the first row in the result set.</returns>
        public object ExecuteScalar(string CommandKey, string DatabaseName)
        {
            return ExecuteScalarCommand(GetDbCommand(CommandKey, DatabaseName));
        }

        #endregion

        #region Private Methods

        private DbCommand GetDbCommand(string commandKey, string databaseName)
        {
            DBCommandLibStruct cmdLibStruct = new DBCommandLibStruct();
            if (commandKey == ConnectionConfig.UserCommandKey)
                cmdLibStruct.CommandText = UserQueryInput;
            else
                cmdLibStruct = DBCommandParser.GetCommandDetails(commandKey);

            cmdLibStruct.DatabaseName = string.IsNullOrEmpty(databaseName) ? string.Empty : databaseName;
            DbCommand command = GetCommand(cmdLibStruct);
            command.CommandType = cmdLibStruct.CommandType;
            command.Parameters.Clear();
            command.CommandText = SetParamPrefix(cmdLibStruct.CommandText);
            return command;
        }

        private DbCommand GetDbCommand(string commandKey, DBCommandParam dbCommandparameter, string databaseName)
        {
            DbCommand command = GetDbCommand(commandKey, databaseName);
            if (dbCommandparameter != null)
                SetDBParameters(command, dbCommandparameter);
            return command;
        }

        private DbCommand GetDbCommand(string commandKey, List<DBCommandParam> dbCommandparameters, string databaseName)
        {
            DbCommand command = GetDbCommand(commandKey, databaseName);
            if (dbCommandparameters != null)
            {
                foreach (DBCommandParam param in dbCommandparameters)
                    SetDBParameters(command, param);
            }
            return command;
        }

        private void SetDBParameters(DbCommand dbCommand, DBCommandParam param)
        {
            if (param.IsCsvParam)
            {
                if (param.CsvList != null)
                {
                    string valueList = string.Empty;
                    for (int i = 0; i < param.CsvList.Count; i++)
                        valueList += (i == 0 ? "'" : ",'") + param.CsvList[i].Trim() + "'";
                    dbCommand.CommandText = dbCommand.CommandText.Replace(SetParamPrefix(param.Name), valueList);
                }
            }
            else
                dbCommand.Parameters.Add(GetParameter(param));
        }

        private void FillDS<T>(DbCommand dbCommand, T Dataset, string TableName) where T : DataSet
        {
            DbDataAdapter adapter = DataAdapter;
            adapter.SelectCommand = dbCommand;
            if (TableName != null && TableName != string.Empty)
                adapter.Fill(Dataset, TableName);
            else
                adapter.Fill(Dataset);
        }

        private void FillDT<T>(DbCommand dbCommand, T Datatable) where T : DataTable
        {
            DbDataAdapter adapter = DataAdapter;
            adapter.SelectCommand = dbCommand;
            adapter.Fill(Datatable);
        }

        private int ExecuteNQuery(DbCommand dbCommand)
        {
            //dbCommand.Connection.Open();
            OpenConnection(dbCommand);
            int returnVal = dbCommand.ExecuteNonQuery();
            CloseConnection(dbCommand);//dbCommand.Connection.Close();
            return returnVal;
        }

        private object ExecuteScalarCommand(DbCommand dbCommand)
        {
            OpenConnection(dbCommand);//dbCommand.Connection.Open();
            object returnVal = dbCommand.ExecuteScalar();
            CloseConnection(dbCommand);//dbCommand.Connection.Close();
            return returnVal;
        }

        /// <summary>
        /// Opens a closed connection.
        /// </summary>
        /// <param name="command">The command to which the connection belongs.</param>
        private void OpenConnection(DbCommand command)
        {
            if (!transactionOn)
            {
                if (command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
            }

        }

        /// <summary>
        /// Closes an open connection.
        /// </summary>
        /// <param name="command">The command to which the connection belongs.</param>
        private void CloseConnection(DbCommand command)
        {
            if (!transactionOn)
            {
                if (command.Connection.State == ConnectionState.Open)
                    command.Connection.Close();
            }
        }

        /// <summary>
        /// Gets an instance of DbConnection.
        /// </summary>
        /// <param name="cmdLibStruct"></param>
        /// <returns></returns>
        private DbConnection GetConnection(DBCommandLibStruct cmdLibStruct)
        {
            DbConnection connection = null;
            ConnectionString = GetConnectionString(cmdLibStruct.DatabaseName);
            //ProviderName = conSettings.ProviderName;
            if (transactionOn)
            {
                //if _connectionWithTransaction is not initialized, then create a connection with 
                //transaction and assign to it.
                if (_connectionWithTransaction == null)
                {
                    _connectionWithTransaction = Connection;
                    _connectionWithTransaction.Open();
                }
                if (_connectionWithTransaction.ConnectionString != ConnectionString)

                    connection = _connectionWithTransaction;
            }
            else
            {
                connection = Connection;
            }
            return connection;
        }

        private static string GetConnectionString(string DatabaseName)
        {
            string connString = ConnectionConfig.GetConnectionStringForLoggedInUser();
            if (!string.IsNullOrEmpty(DatabaseName))
                connString += string.Format("Database={0};", DatabaseName);
            return connString;
        }

        /// <summary>
        /// Gets an instance of the command.
        /// </summary>
        /// <param name="cmdLibStruct"></param>
        /// <returns></returns>
        private DbCommand GetCommand(DBCommandLibStruct cmdLibStruct)
        {
            DbCommand command;
            DbConnection connection = GetConnection(cmdLibStruct);
            if (transactionOn)
            {
                //If _commandWithTransaction is not initialized, then create it and open the transaction.
                if (_commandWithTransaction == null)
                {
                    _commandWithTransaction = connection.CreateCommand();
                    _commandWithTransaction.Transaction = connection.BeginTransaction();
                }
                command = _commandWithTransaction;
            }
            else
            {
                command = connection.CreateCommand();
            }
            if (CommandTimeout.HasValue)
                command.CommandTimeout = CommandTimeout.Value;
            return command;
        }

        #endregion

        #region Abstract methods / properties

        internal abstract DbDataAdapter DataAdapter
        {
            get;
        }

        internal abstract DbConnection Connection
        {
            get;
        }

        internal abstract string SetParamPrefix(string text);

        internal abstract IDbDataParameter GetParameter(DBCommandParam dbCommandParam);

        #endregion
    }
}