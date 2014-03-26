using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.eforceglobal.DBAdmin.Utils;

public partial class Top : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            imgLogo.ImageUrl = UIManager.GetThemedURL(this, @"/img/app/SQLDBAdminLogo.jpg"); 
    }
    protected void lnkDisconnect_Click(object sender, EventArgs e)
    {
        SessionManager.ClearAll();
        FormsAuthentication.SignOut();
        string redirectUrl = FormsAuthentication.LoginUrl;
        string js = string.Format("javascript:window.parent.navigate('{0}');", redirectUrl);
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Redirect_To_Home", js, true);
    }
}
