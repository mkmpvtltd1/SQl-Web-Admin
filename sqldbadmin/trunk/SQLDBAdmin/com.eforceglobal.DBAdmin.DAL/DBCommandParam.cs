using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;

namespace com.eforceglobal.DBAdmin.DAL
{
    public class DBCommandParam
    {
        string _name;
        object _value;
        DbType _dbType;
        int _size;
        List<string> _csvList;
        bool _isCsvParam;

        public DBCommandParam(string name, object value,DbType dbType)
        {
            _name = name;
            _value = value;
            _dbType = dbType;
        }
        public DBCommandParam(string name, object value, DbType dbType, int size)
        {
            _name = name;
            _value = value;
            _dbType = dbType;
            _size = size;
        }
        public DBCommandParam(string name, List<string> csvList)
        {
            _name = name;
            _csvList = csvList;
            _isCsvParam = true;
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public DbType DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }
        public List<string> CsvList
        {
            get { return _csvList; }
            set { _csvList = value; }
        }
        public bool IsCsvParam
        {
            get { return _isCsvParam; }
        }
    }
}