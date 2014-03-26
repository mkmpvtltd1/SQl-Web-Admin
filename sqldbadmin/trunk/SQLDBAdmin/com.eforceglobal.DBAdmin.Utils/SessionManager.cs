using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using com.eforceglobal.DBAdmin.BusinessObjects;

public class SessionManager
{
    private class SessionVariableNames
    {
        public const string CurrentUser = "CurrentUser";
    }

    public static DBAdminUser CurrentUser
    {
        get
        {
            DBAdminUser user = null;
            try
            {

                Object sessionObject = HttpContext.Current.Session[SessionVariableNames.CurrentUser];
                if (sessionObject != null)
                {
                    user = (DBAdminUser)sessionObject;
                }
            }
            catch (Exception)
            {
                user = null;
            }
            return user;
        }
        set
        {
            HttpContext.Current.Session[SessionVariableNames.CurrentUser] = value;
        }
    }

    public static void LoginUserAndSetCookie(DBAdminUser user, bool isPersistent)
    {
        var Authticket = new FormsAuthenticationTicket(
                                                        1,
                                                        user.UserName,
                                                        DateTime.Now,
                                                        DateTime.Now.AddMinutes(30),
                                                        isPersistent,
                                                        user.Allow,
                                                        FormsAuthentication.FormsCookiePath);

        string hash = FormsAuthentication.Encrypt(Authticket);

        var Authcookie = new HttpCookie
          (FormsAuthentication.FormsCookieName, hash);

        if (Authticket.IsPersistent)
            Authcookie.Expires = Authticket.Expiration;

        HttpContext.Current.Response.Cookies.Add(Authcookie);
        CurrentUser = user;
    }
    public static void LogOffUser()
    {
        FormsAuthentication.SignOut();
    }

    /// <summary>
    /// Clear all the session variables
    /// </summary>
    public static void ClearAll()
    {
        CurrentUser = null;
    }
}

