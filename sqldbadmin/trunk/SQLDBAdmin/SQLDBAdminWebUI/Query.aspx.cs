using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.eforceglobal.DBAdmin.Constants;
using com.eforceglobal.DBAdmin.Facades;
using com.eforceglobal.DBAdmin.Facades.Contracts;

public partial class Query : Page
{
    private readonly IDatabaseFacade facade = new DatabaseFacade();

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Visible = false;
        if (!Page.IsPostBack)
        {
            PopulateDatabaseList();
            if (Request.Params["q"] != null)
            {
                string query = Request.Params["q"];
                string databaseName = Request.Params["d"];
                ShowQueryResult(databaseName, query);
            }
        }
        if (!string.IsNullOrEmpty(hiddenImageToggleUrl.Value))
        {
            imgToggle.ImageUrl = hiddenImageToggleUrl.Value;
            imgToggle.ToolTip = hiddenImageToggleToolTip.Value;
        }
        else
        {
            imgToggle.ImageUrl = "~/images/reducenavbar.gif";
            imgToggle.ToolTip = "Expand";
        }

    }

    private void PopulateDatabaseList()
    {
        string selectedDatabase = SessionManager.CurrentUser.ConnectedDatabase;
        if (!string.IsNullOrEmpty(selectedDatabase))
        {
            ListItem item = new ListItem(selectedDatabase, selectedDatabase);
            ddlDatabases.Items.Add(item);
            return;
        }
        DataTable tblDatabases = facade.GetDatabaseSchema();
        foreach (DataRow row in tblDatabases.Rows)
        {
            ListItem item = new ListItem(row["Database_Name"].ToString(), row["Database_Name"].ToString());
            ddlDatabases.Items.Add(item);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ShowQueryResult(ddlDatabases.SelectedValue, txtQuery.Text.Trim());
    }

    private void ShowQueryResult(string databaseName, string query)
    {
        if (!string.IsNullOrEmpty(query))
        {
            DataSet dsResult = new DataSet();
            try
            {
                dsResult = facade.GetResultUserQuery(databaseName, query);
            }
            catch (SqlException ex)
            {
                string errorMessage = String.Format("Sql Server Error: Msg {0}, Level {1}, State {2}, Line {3}<br>\n{4}",
                                                    new object[] { ex.Number, ex.Class, ex.State, ex.LineNumber, ex.Message });
                ShowErrorMessage(errorMessage);
            }
            try
            {
                foreach (DataTable result in dsResult.Tables)
                {
                    if (result.Rows.Count == 0)
                    {
                        Label lblInfo = new Label
                                            {
                                                Text = "No records found!"
                                            };
                        pnlResult.Controls.Add(lblInfo);
                        GiveVerticalSpace();
                        continue;
                    }
                    GridView grdResult = new GridView
                    {
                        DataSource = result,
                    };
                    pnlResult.Controls.Add(grdResult);
                    grdResult.DataBind();
                    grdResult.Width = pnlResult.Width;
                    GiveVerticalSpace();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
            finally
            {
                GC.Collect();
            }
        }
    }

    private void GiveVerticalSpace()
    {
        LiteralControl litResult = new LiteralControl { Text = "<br/><br/>" };
        pnlResult.Controls.Add(litResult);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtQuery.Text = string.Empty;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtQuery.Text.Trim()))
        {
            //convert the query to byte[]
            byte[] content = Encoding.ASCII.GetBytes(txtQuery.Text.Trim());
            //save the file by download dialog
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=SQLQuery1.sql");
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(content);
            Response.Flush();
            Response.End();
        }
    }

    private void OpenFile()
    {
        try
        {
            string fileName = fileUploadQuery.FileName;
            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Server.MapPath(Request.ApplicationPath + "/" + Paths.FilePathToStore + "/" + fileName);
                if (fileName.Substring(fileName.LastIndexOf('.')) != ".sql")
                {
                    ShowErrorMessage("Only .sql files are supported!");
                    return;
                }
                fileUploadQuery.SaveAs(filePath);

                if (fileUploadQuery.HasFile)
                {
                    StreamReader sr = new StreamReader(filePath);
                    txtQuery.Text = sr.ReadToEnd();
                    sr.Close();
                    if (File.Exists(filePath)) File.Delete(filePath);
                }
            }
            else
            {
                ShowErrorMessage("No file selected to open!");
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }
    }

    private void ShowErrorMessage(string errorMessage)
    {
        lblError.Visible = true;
        lblError.Text = errorMessage;
    }

    protected void btnWriteFile_Click(object sender, EventArgs e)
    {
        OpenFile();
    }
}
