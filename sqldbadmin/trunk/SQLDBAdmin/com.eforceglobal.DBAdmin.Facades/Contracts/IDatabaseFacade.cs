using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace com.eforceglobal.DBAdmin.Facades.Contracts
{
    public interface IDatabaseFacade
    {
        DataTable GetDatabaseSchema();
        DataTable GetViews(string DatabaseName);
        DataTable GetTables(string DatabaseName);
        DataTable GetStoredProcs(string DatabaseName);       
        DataTable GetColumns(string DatabaseName, string TableName);
        DataTable GetTableValuedFunctions(string DatabaseName);
        DataTable GetCLRTableValuedFunctions(string DatabaseName);
        DataTable GetScalarValuedFunctions(string DatabaseName);
        DataSet GetResultUserQuery(string DatabaseName, string Query);
    }
}
