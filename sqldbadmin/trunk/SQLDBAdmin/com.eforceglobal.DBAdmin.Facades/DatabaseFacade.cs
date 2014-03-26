using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using com.eforceglobal.DBAdmin.Constants;
using com.eforceglobal.DBAdmin.DAL;
using com.eforceglobal.DBAdmin.Facades.Contracts;
using com.eforceglobal.DBAdmin.Utils;

namespace com.eforceglobal.DBAdmin.Facades
{
    public class DatabaseFacade : IDatabaseFacade
    {
        readonly DBHelper dbHelper = DALFactory.GetDBHelper();

        public DataTable GetDatabaseSchema()
        {
            DataTable tblDatabases = new DataTable();
            dbHelper.FillDataTable("AvailableDatabases", string.Empty, tblDatabases);
            return tblDatabases;
        }

        public DataTable GetViews(string DatabaseName)
        {
            DataTable tblViews = new DataTable();
            dbHelper.FillDataTable("AllViews", DatabaseName, tblViews);
            return tblViews;
        }

        public DataTable GetTables(string DatabaseName)
        {
            DataTable tblTables = new DataTable();
            dbHelper.FillDataTable("AllTables", DatabaseName, tblTables);
            return tblTables;
        }

        public DataTable GetStoredProcs(string DatabaseName)
        {
            DataTable tblStoredProcs = new DataTable();
            dbHelper.FillDataTable("AllStoredProcs", DatabaseName, tblStoredProcs);
            dbHelper.FillDataTable("AllCLRStoredProcs", DatabaseName, tblStoredProcs);
            return tblStoredProcs;
        }

        public DataTable GetColumns(string DatabaseName, string TableName)
        {
            DataTable tblColumns = new DataTable();
            DBCommandParam paramTableXml = new DBCommandParam("?TableName", TableName, DbType.String);
            dbHelper.FillDataTable("AllColumns", DatabaseName, paramTableXml, tblColumns);
            return tblColumns;
        }

        public DataTable GetTableValuedFunctions(string DatabaseName)
        {
            DataTable tblTableValuedFunctions = new DataTable();
            dbHelper.FillDataTable("TableValuedFunctions", DatabaseName, tblTableValuedFunctions);
            dbHelper.FillDataTable("CLRTableValuedFunctions", DatabaseName, tblTableValuedFunctions);
            return tblTableValuedFunctions;
        }

        public DataTable GetCLRTableValuedFunctions(string DatabaseName)
        {
            DataTable tblCLRTableValuedFunctions = new DataTable();
            dbHelper.FillDataTable("CLRTableValuedFunctions", DatabaseName, tblCLRTableValuedFunctions);
            return tblCLRTableValuedFunctions;
        }

        public DataTable GetScalarValuedFunctions(string DatabaseName)
        {
            DataTable tblScalarValuedFunctions = new DataTable();
            dbHelper.FillDataTable("ScalarValuedFunctions", DatabaseName, tblScalarValuedFunctions);
            return tblScalarValuedFunctions;
        }

        public DataSet GetResultUserQuery(string DatabaseName, string Query)
        {
            DataSet dsResult = new DataSet();
            dbHelper.UserQueryInput = Query;
            dbHelper.FillDataSet(ConnectionConfig.UserCommandKey, DatabaseName, dsResult);
            return dsResult;
        }
    }
}
