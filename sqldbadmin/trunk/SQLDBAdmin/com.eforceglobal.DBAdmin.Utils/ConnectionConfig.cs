using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;

namespace com.eforceglobal.DBAdmin.Utils
{
    public class ConnectionConfig
    {
        public const string UserCommandKey = "UserCommandKey";

        public static List<string> GetAllServerIPs()
        {
            List<string> lstIPs = new List<string>();
            XPathNavigator xPathNavigator = GetServersNodeFromCache();
            XPathNodeIterator xPathNodeIterator = xPathNavigator.Select("configuration/servers/server");
            XmlNamespaceManager nm = new XmlNamespaceManager(xPathNavigator.NameTable);
            while (xPathNodeIterator.MoveNext())
            {
                string address = xPathNodeIterator.Current.GetAttribute("address", nm.DefaultNamespace);
                string name = xPathNodeIterator.Current.GetAttribute("name", nm.DefaultNamespace);
                lstIPs.Add(string.Format("{0} ({1})", name, address));
            }
            return lstIPs;
        }
        private static XPathNavigator GetServersNodeFromCache()
        {
            XPathNavigator xNav = (XPathNavigator)HttpRuntime.Cache.Get("Servers");
            if (xNav == null)
            {
                string fileName = HttpContext.Current.Request.PhysicalApplicationPath + "config/Connections.config";
                XPathDocument xDoc = new XPathDocument(fileName);
                xNav = xDoc.CreateNavigator();
                HttpRuntime.Cache.Add("Servers", xNav, new System.Web.Caching.CacheDependency(fileName), DateTime.Now.AddDays(7),
                                      TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);
            }
            return xNav;
        }
        public static string GetConnectionString(string address)
        {
            XPathNavigator xPathNavigator = GetServersNodeFromCache();
            XPathNodeIterator xPathNodeIterator = xPathNavigator.Select("configuration/servers/server[@address='" + address + "']");
            XmlNamespaceManager nm = new XmlNamespaceManager(xPathNavigator.NameTable);
            xPathNodeIterator.MoveNext();
            string username = xPathNodeIterator.Current.GetAttribute("username", nm.DefaultNamespace);
            string password = xPathNodeIterator.Current.GetAttribute("password", nm.DefaultNamespace);
            string connectionString = string.Format("Server={0};uid={1};pwd={2};", address, username, password);
            return connectionString;
        }
        public static string GetConnectionStringForLoggedInUser()
        {
            return SessionManager.CurrentUser.ConnectionString;
        }

    }
}
