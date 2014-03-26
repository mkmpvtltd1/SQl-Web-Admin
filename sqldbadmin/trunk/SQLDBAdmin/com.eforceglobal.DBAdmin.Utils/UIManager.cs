using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using com.eforceglobal.DBAdmin.Constants;

namespace com.eforceglobal.DBAdmin.Utils
{
    public static class UIManager
    {
        public static string GetThemedURL(Page callingPage, string imagePath)
        {
            return @"~/App_Themes/" + callingPage.Theme + imagePath;
        }        
    }
}
