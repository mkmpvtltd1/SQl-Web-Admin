using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace com.eforceglobal.DBAdmin.DAL
{
    internal struct DBCommandLibStruct
    {
        internal string DatabaseName { get; set; }

        internal string CommandText { get; set; }

        internal string CommandConnectionKey { get; set; }

        internal CommandType CommandType { get; set; }
    }
}