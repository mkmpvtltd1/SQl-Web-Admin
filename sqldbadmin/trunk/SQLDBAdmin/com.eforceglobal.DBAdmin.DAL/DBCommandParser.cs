using System;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Web;
using com.eforceglobal.DBAdmin.Constants;
using System.Data;
using System.Configuration;
using com.eforceglobal.DBAdmin.DAL;

namespace com.eforceglobal.DBAdmin.DAL
{
    internal class DBCommandParser
    {
        internal static DBCommandLibStruct GetCommandDetails(string Key)
        {
            XPathNavigator xNavigator = GetDBCommandLibNavigator();
            //xNavigator.MoveToRoot();
            XPathNodeIterator oIterator = xNavigator.Select("Commands");

            DBCommandLibStruct cmdLibStruct = new DBCommandLibStruct();
            //while (oIterator.MoveNext())
            //{
            //    oIterator.Current.MoveToAttribute("connectionKey", "");
            //    cmdLibStruct.ConnectionKey = oIterator.Current.Value;
            //}
            //xNavigator.MoveToRoot();
            oIterator = xNavigator.Select("Commands/Command[@key='" + Key + "']");
            //string appMode = ConfigurationManager.AppSettings["ApplicationMode"];
            while (oIterator.MoveNext())
            {
                //if (appMode.Equals("integrated") && oIterator.Current.MoveToAttribute("integrationCmd", ""))
                //{
                //    cmdLibStruct.CommandText = oIterator.Current.Value.Trim();
                //    oIterator.Current.MoveToParent();
                //}
                //else
                cmdLibStruct.CommandText = oIterator.Current.Value.Trim();
                //if (oIterator.Current.MoveToAttribute("connectionKey", ""))
                //{
                //    cmdLibStruct.CommandConnectionKey = oIterator.Current.Value;
                //    oIterator.Current.MoveToParent();
                //}
                if (oIterator.Current.MoveToAttribute("commandType", "")
                    && Enum.IsDefined(typeof(CommandType), oIterator.Current.Value))
                    cmdLibStruct.CommandType = (CommandType)Enum.Parse(typeof(CommandType), oIterator.Current.Value);
                else
                    cmdLibStruct.CommandType = CommandType.Text;
            }

            return cmdLibStruct;
        }

        private static XPathNavigator GetDBCommandLibNavigator()
        {
            XPathNavigator xNav = (XPathNavigator)HttpRuntime.Cache.Get("DBCommandLibrary");
            if (xNav == null)
            {
                string fileName = Paths.AssemblyPath + "DBCommandLibrary.xml";
                XPathDocument xDoc = new XPathDocument(fileName);
                xNav = xDoc.CreateNavigator();
                HttpRuntime.Cache.Add("DBCommandLibrary", xNav, null, DateTime.Now.AddDays(7),
                                      TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);
            }
            return xNav;
        }
    }
}