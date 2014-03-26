using com.eforceglobal.DBAdmin.DAL;

namespace com.eforceglobal.DBAdmin.DAL
{
    public class DALFactory
    {
        public static DBHelper GetDBHelper()
        {
            return new CommonHelper();
        }
    }
}