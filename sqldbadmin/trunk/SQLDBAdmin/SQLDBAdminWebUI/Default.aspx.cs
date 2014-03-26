using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.eforceglobal.DBAdmin.Utils;
using com.eforceglobal.DBAdmin.BusinessObjects;

public partial class _Default : System.Web.UI.Page
{
    private string UserName
    {
        get
        {
            return txtUserName.Text.Trim();
        }
    }
    private string Password
    {
        get
        {
            return txtPassword.Text.Trim();
        }
    }
    private string SelectedServerAddress
    {
        get
        {
            string value = ddlServers.SelectedValue;
            return value.Substring(value.LastIndexOf('(') + 1, value.IndexOf(')') - value.LastIndexOf('(') - 1);
        }
    }
    private string SelectedDatabase
    {
        get
        {
            return txtDatabase.Text.Trim();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblErrorMessage.Visible = false;
        if (!Page.IsPostBack)
        {
            PopulateServerDropdown();
        }
    }

    private void PopulateServerDropdown()
    {
        ddlServers.DataSource = ConnectionConfig.GetAllServerIPs();
        ddlServers.DataBind();
        if (ddlServers.Items.Count == 0)
        {
            ShowErrorMessage("There are no servers defined in the connections config file. You can't login");
            btnLogin.Enabled = false;
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        DBAdminUser user;
        try
        {
            user = UserXMLParser.GetUser(UserName, Password);
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
            return;
        }
        if (user == null)
        {
            ShowErrorMessage("Invalid User Name or Password");
        }
        else
        {
            string conn = ConnectionConfig.GetConnectionString(SelectedServerAddress);
            if (CheckConnectionString(conn))
            {
                user.ConnectionString = conn;
                user.ConnectedServer = SelectedServerAddress;
                user.ConnectedDatabase = SelectedDatabase;
                SessionManager.LoginUserAndSetCookie(user, false);
                string redirectUrl = FormsAuthentication.GetRedirectUrl(user.UserName, false);
                Response.Redirect(redirectUrl);
            }
        }

    }

    private bool CheckConnectionString(string conn)
    {
        if (!string.IsNullOrEmpty(SelectedDatabase))
            conn += "Database=" + SelectedDatabase;
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                sqlConnection.Open();
            }
        }
        catch (Exception)
        {
            ShowErrorMessage("Connection string is not working for " + ddlServers.SelectedValue);
            return false;
        }
        return true;
    }

    private void ShowErrorMessage(string message)
    {
        lblErrorMessage.Text = message;
        lblErrorMessage.Visible = true;
    }
}
