using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.eforceglobal.DBAdmin.BusinessObjects
{
    public class DBAdminUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Allow { get; set; }
        public string ConnectionString { get; set; }
        public string ConnectedServer { get; set; }
        public string ConnectedDatabase { get; set; }
    }
}