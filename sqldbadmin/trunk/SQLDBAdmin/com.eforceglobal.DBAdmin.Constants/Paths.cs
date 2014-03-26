using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.eforceglobal.DBAdmin.Constants
{
    public class Paths
    {
        public static string AssemblyPath
        {
            get
            {
                if (System.AppDomain.CurrentDomain.RelativeSearchPath != null)
                    return System.AppDomain.CurrentDomain.RelativeSearchPath + "\\";
                return System.AppDomain.CurrentDomain.BaseDirectory + "\\";
            }
        }
        public static string FilePathToStore
        {
            get
            {
                return "File";
            }
        }       
    }
}
