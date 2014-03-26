using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using com.eforceglobal.DBAdmin.BusinessObjects;

namespace com.eforceglobal.DBAdmin.Utils
{
    public class UserXMLParser
    {
        public static DBAdminUser GetUser(string UserName, string Password)
        {
            DBAdminUser user = new DBAdminUser();
            XPathNavigator xNavigator = GetUsersNodeFromCache();
            XPathNodeIterator iterator =
                xNavigator.Select("Users/User[@name='" + UserName + "' and @password='" + Password + "']");

            //if user not found
            if (iterator.Count == 0) return null;

            //if more than one user found with same username and password
            if (iterator.Count > 1)
                throw new Exception("Multiple Users with same name and password cannot exists");
            
            XmlNamespaceManager ns = new XmlNamespaceManager(xNavigator.NameTable);
            iterator.MoveNext();
            string allow = iterator.Current.GetAttribute("allow", ns.DefaultNamespace);

            //populate user object
            user.UserName = UserName;
            user.Password = Password;
            user.Allow = allow;

            //add user to session
            SessionManager.CurrentUser = user;
            return user;
        }

        private static XPathNavigator GetUsersNodeFromCache()
        {
            XPathNavigator xNav = (XPathNavigator)HttpRuntime.Cache.Get("Users");
            if (xNav == null)
            {
                string fileName = HttpContext.Current.Request.PhysicalApplicationPath + "config/Users.xml";
                XPathDocument xDoc = new XPathDocument(fileName);
                xNav = xDoc.CreateNavigator();
                HttpRuntime.Cache.Add("Users", xNav, new System.Web.Caching.CacheDependency(fileName), DateTime.Now.AddDays(7),
                                      TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);
            }
            return xNav;
        }
    }
}