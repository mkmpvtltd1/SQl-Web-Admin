using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using com.eforceglobal.DBAdmin.Facades;
using com.eforceglobal.DBAdmin.Facades.Contracts;
using com.eforceglobal.DBAdmin.Utils;

public partial class DatabaseList : Page
{
    private readonly IDatabaseFacade facade = new DatabaseFacade();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            PopulateInitialTreeView();
        }
    }

    #region DataBase Nodes
    private void PopulateDataBaseNodes(TreeNode node)
    {
        string selectedDatabase = SessionManager.CurrentUser.ConnectedDatabase;
        if (!string.IsNullOrEmpty(selectedDatabase))
        {
            TreeNode nodeDatabase = new TreeNode(selectedDatabase, selectedDatabase)
            {
                ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/database.gif"),
                SelectAction = TreeNodeSelectAction.Expand
            };
            nodeDatabase.PopulateOnDemand = true;
            nodeDatabase.CollapseAll();
            node.ChildNodes.Add(nodeDatabase);
            return;
        }
        List<DataRow> rows = GetDatabaseNodes(node.Value);
        if (rows != null)
        {
            foreach (DataRow row in rows)
            {
                string databaseName = row["DATABASE_NAME"].ToString();
                TreeNode nodeDatabase = new TreeNode(databaseName, databaseName)
                                            {
                                                ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/database.gif"),
                                                SelectAction = TreeNodeSelectAction.Expand
                                            };
                nodeDatabase.PopulateOnDemand = true;
                nodeDatabase.CollapseAll();
                node.ChildNodes.Add(nodeDatabase);
            }
        }
    }

    private List<DataRow> GetDatabaseNodes(string parentNode)
    {
        DataTable databases = facade.GetDatabaseSchema();
        List<DataRow> rows = null;
        if (parentNode.ToLower() == "systemdatabases")
            rows = databases.Rows.Cast<DataRow>().Where(row => IsSystemDatabases(row["Database_Name"].ToString())).ToList();
        else if (parentNode.ToLower() == "userdatabases")
            rows = databases.Rows.Cast<DataRow>().Where(row => !IsSystemDatabases(row["Database_Name"].ToString())).ToList();

        return rows;
    }
    #endregion

    #region Basic Folders under Databases
    private void CreateBasicFolders(TreeNode nodeDatabase)
    {
        string databaseName = nodeDatabase.Value;
        TreeNode nodeTables = new TreeNode("Tables", "Tables|" + databaseName)
                                  {
                                      SelectAction = TreeNodeSelectAction.Expand,
                                      ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/folder.gif")
                                  };
        nodeTables.PopulateOnDemand = true;
        nodeTables.CollapseAll();

        TreeNode nodeViews = new TreeNode("Views", "Views|" + databaseName)
                                 {
                                     SelectAction = TreeNodeSelectAction.Expand,
                                     ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/folder.gif")
                                 };
        nodeViews.PopulateOnDemand = true;
        nodeViews.CollapseAll();

        TreeNode nodeProgrammability = new TreeNode("Programmability", "Programmability|" + databaseName)
                                           {
                                               SelectAction = TreeNodeSelectAction.Expand,
                                               ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/folder.gif")
                                           };
        nodeProgrammability.PopulateOnDemand = true;
        nodeProgrammability.CollapseAll();

        nodeDatabase.ChildNodes.Add(nodeTables);
        nodeDatabase.ChildNodes.Add(nodeViews);
        nodeDatabase.ChildNodes.Add(nodeProgrammability);
    }
    #endregion

    #region Tables
    private void CreateTables(string databaseName, TreeNode nodeTables)
    {
        DataTable tables = facade.GetTables(databaseName);
        foreach (DataRow table in tables.Rows)
        {
            string tableName = table["name"].ToString();
            TreeNode nodeTable = new TreeNode(tableName, "TableNames|" + databaseName + "|" + tableName)
                                     {
                                         SelectAction = TreeNodeSelectAction.Expand,
                                         ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/table.gif")
                                     };
            AppendExtraHTML(nodeTable);
            //CreateColumns(nodeTable, tableName, databaseName);
            nodeTable.Collapse();
            nodeTable.PopulateOnDemand = true;
            nodeTables.ChildNodes.Add(nodeTable);
        }
    }

    private void AppendExtraHTML(TreeNode node)
    {
        string tableName = node.Value.Split('|')[2];
        string databaseName = node.Value.Split('|')[1];
        string query = string.Format("select * from {0}", tableName);
        StringBuilder sb = new StringBuilder();
        sb.Append(Server.HtmlDecode(string.Format("&nbsp;<a href='Query.aspx?d={0}&q={1}' target=\"content\"><img title='Open Table' src='images/content.gif' style='border-width:0px;'/></a>", databaseName, query)));
        node.Text += sb.ToString();
    }

    #endregion

    #region Columns
    private void CreateColumns(TreeNode nodeTable, string tableName, string databaseName)
    {
        DataTable tblColumns = facade.GetColumns(databaseName, tableName);
        TreeNode nodeColumnFolder = new TreeNode("Columns", "Columns|" + databaseName + "|" + tableName)
        {
            SelectAction = TreeNodeSelectAction.Expand,
            ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/folder.gif")
        };

        foreach (DataRow column in tblColumns.Rows)
        {
            string columnName = column["COLUMN_NAME"].ToString();
            TreeNode nodeColumn = new TreeNode(columnName, columnName + "|" + databaseName + "|" + tableName)
                                      {
                                          SelectAction = TreeNodeSelectAction.None,
                                          ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/column.gif")
                                      };
            nodeColumnFolder.ChildNodes.Add(nodeColumn);
        }
        nodeTable.ChildNodes.Add(nodeColumnFolder);
    }
    #endregion

    #region Views
    private void CreateViews(string databaseName, TreeNode nodeViews)
    {
        DataTable views = facade.GetViews(databaseName);
        TreeNode nodeSystemView = new TreeNode("System Views", "System Views|" + databaseName);
        nodeSystemView.CollapseAll();
        nodeSystemView.SelectAction = TreeNodeSelectAction.Expand;
        nodeSystemView.ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/folder.gif");
        nodeViews.ChildNodes.Add(nodeSystemView);

        foreach (DataRow view in views.Rows)
        {
            string viewName = view["name"].ToString();
            int viewId = int.Parse(view["object_id"].ToString());
            TreeNode nodeView = new TreeNode(viewName, viewName + "|" + databaseName);
            nodeView.SelectAction = TreeNodeSelectAction.None;
            nodeView.ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/view.gif");
            if (IsSystemView(viewId))
            {
                nodeSystemView.ChildNodes.Add(nodeView);
            }
            else
                nodeViews.ChildNodes.Add(nodeView);
        }
    }
    #endregion

    #region Programmability
    private void CreateProgrammabilityFolders(string databaseName, TreeNode nodeProg)
    {
        //for Stored Procedures
        TreeNode nodeStoredProcedures = new TreeNode("Stored Procedures", "StoredProcedures|" + databaseName)
        {
            ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/folder.gif"),
            SelectAction = TreeNodeSelectAction.Expand
        };
        nodeStoredProcedures.PopulateOnDemand = true;
        nodeStoredProcedures.CollapseAll();

        //for Functions
        TreeNode nodeFunctions = new TreeNode("Functions", "Functions|" + databaseName)
        {
            ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/folder.gif"),
            SelectAction = TreeNodeSelectAction.Expand
        };
        nodeFunctions.PopulateOnDemand = true;
        nodeFunctions.CollapseAll();

        nodeProg.ChildNodes.Add(nodeStoredProcedures);
        nodeProg.ChildNodes.Add(nodeFunctions);
    }
    #endregion

    #region Helper Methods
    private void PopulateInitialTreeView()
    {
        string serverIP = SessionManager.CurrentUser.ConnectedServer;
        TreeNode nodeServerIP = new TreeNode(serverIP, serverIP)
        {
            ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/server.gif"),
            SelectAction = TreeNodeSelectAction.Expand
        };
        nodeServerIP.Expand();
        //for system databases
        if (string.IsNullOrEmpty(SessionManager.CurrentUser.ConnectedDatabase))
        {
            TreeNode nodeSystemDatabase = new TreeNode("System Databases", "SystemDatabases")
            {
                ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/folder.gif"),
                SelectAction = TreeNodeSelectAction.Expand
            };
            nodeSystemDatabase.PopulateOnDemand = true;
            nodeSystemDatabase.CollapseAll();
            nodeServerIP.ChildNodes.Add(nodeSystemDatabase);
        }
        //for user databases
        TreeNode nodeUserDatabase = new TreeNode("User Databases", "UserDatabases")
        {
            ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/folder.gif"),
            SelectAction = TreeNodeSelectAction.Expand
        };
        nodeUserDatabase.PopulateOnDemand = true;
        nodeUserDatabase.CollapseAll();

        nodeServerIP.ChildNodes.Add(nodeUserDatabase);
        treeViewDBList.Nodes.Add(nodeServerIP);
    }
    protected void PopulateNode(object sender, TreeNodeEventArgs e)
    {
        string[] nodeInfo;
        string databaseName;
        //first set the populate on demand = false, 
        //so it will not cause another NodePopulate when the node gets collapsed and later expanded, since it stores it state in the View State together with the populated tree nodes.
        e.Node.PopulateOnDemand = false;
        // Call the appropriate method to populate a node at a particular level.
        switch (e.Node.Depth)
        {
            case 1:
                // Populate the first-level nodes.                         
                PopulateDataBaseNodes(e.Node);
                break;
            case 2:
                // Populate the second-level nodes.
                //create the basic folders under the databases
                CreateBasicFolders(e.Node);
                break;
            case 3:
                // Populate the third-level nodes.
                //For Tables or Views or Programmability
                nodeInfo = e.Node.Value.Split('|');
                databaseName = nodeInfo[1];
                if (nodeInfo[0].ToLower() == "tables")
                {
                    CreateTables(databaseName, e.Node);
                }
                else if (nodeInfo[0].ToLower() == "views")
                {
                    CreateViews(databaseName, e.Node);
                }
                else if (nodeInfo[0].ToLower() == "programmability")
                {
                    CreateProgrammabilityFolders(databaseName, e.Node);
                }

                break;
            case 4:
                //Populate the fourth-level nodes                
                nodeInfo = e.Node.Value.Split('|');
                databaseName = nodeInfo[1];
                if (nodeInfo[0].ToLower() == "storedprocedures")
                {
                    CreateStoredProcedures(databaseName, e.Node);
                }
                else if (nodeInfo[0].ToLower() == "functions")
                {
                    CreateFunctions(databaseName, e.Node);
                }
                else if (nodeInfo[0].ToLower() == "tablenames")
                {
                    string tableName = nodeInfo[2];
                    CreateColumns(e.Node, tableName, databaseName);
                }
                break;
            default:
                // Do nothing.
                break;
        }
    }
    private void CreateStoredProcedures(string databaseName, TreeNode storeProcNode)
    {
        TreeNode nodeSystemSP = new TreeNode("System Stored Procedures", "SystemStoredProcedures|" + databaseName)
        {
            ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/folder.gif"),
            SelectAction = TreeNodeSelectAction.Expand
        };
        nodeSystemSP.CollapseAll();

        storeProcNode.ChildNodes.Add(nodeSystemSP);

        DataTable tblSp = facade.GetStoredProcs(databaseName);
        foreach (DataRow row in tblSp.Rows)
        {
            int objId = Convert.ToInt32(row["object_id"].ToString());
            string type = row["type_desc"].ToString();
            TreeNode nodeStoredProcedures = new TreeNode(row["name"].ToString(), row["name"] + "|" + databaseName)
            {
                SelectAction = TreeNodeSelectAction.Expand
            };
            nodeStoredProcedures.ImageUrl = type.Equals("CLR_STORED_PROCEDURE") ? UIManager.GetThemedURL(this, @"/img/treeView/clr_sp.gif") : UIManager.GetThemedURL(this, @"/img/treeView/sp.gif");

            nodeStoredProcedures.CollapseAll();
            if (IsSystemStoredProc(objId))
            {
                nodeSystemSP.ChildNodes.Add(nodeStoredProcedures);
            }
            else
            {
                storeProcNode.ChildNodes.Add(nodeStoredProcedures);
            }
        }
    }
    private void CreateFunctions(string databaseName, TreeNode funcNode)
    {
        //add folder node Table-valued Functions
        TreeNode nodeTableValuedFn = new TreeNode("Table-valued Functions", "TableValuedFunctions|" + databaseName)
        {
            ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/folder.gif"),
            SelectAction = TreeNodeSelectAction.Expand
        };
        nodeTableValuedFn.CollapseAll();

        //add folder node Scalar-valued Functions
        TreeNode nodeScalarValuedFn = new TreeNode("Scalar-valued Functions", "ScalarValuedFunctions|" + databaseName)
        {
            ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/folder.gif"),
            SelectAction = TreeNodeSelectAction.Expand
        };
        nodeScalarValuedFn.CollapseAll();

        //add these to function node
        funcNode.ChildNodes.Add(nodeTableValuedFn);
        funcNode.ChildNodes.Add(nodeScalarValuedFn);

        //add child nodes of Table-valued Functions
        DataTable tblTableValuedFn = facade.GetTableValuedFunctions(databaseName);
        foreach (DataRow row in tblTableValuedFn.Rows)
        {
            TreeNode nodeChildTableValuedFn = new TreeNode(row["name"].ToString(), row["name"] + "|" + databaseName)
            {
                SelectAction = TreeNodeSelectAction.Expand
            };
            nodeChildTableValuedFn.ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/function.gif");

            nodeChildTableValuedFn.CollapseAll();
            nodeTableValuedFn.ChildNodes.Add(nodeChildTableValuedFn);
        }

        //add child nodes of Scalar-valued Functions
        DataTable tblScalarValudFn = facade.GetScalarValuedFunctions(databaseName);
        foreach (DataRow row in tblScalarValudFn.Rows)
        {
            TreeNode nodeChildScalarValuedFn = new TreeNode(row["name"].ToString(), row["name"] + "|" + databaseName)
            {
                SelectAction = TreeNodeSelectAction.Expand
            };
            nodeChildScalarValuedFn.ImageUrl = UIManager.GetThemedURL(this, @"/img/treeView/function.gif");

            nodeChildScalarValuedFn.CollapseAll();
            nodeScalarValuedFn.ChildNodes.Add(nodeChildScalarValuedFn);
        }
    }
    private static bool IsSystemDatabases(string databaseName)
    {
        if (databaseName.ToLower() == "master" || databaseName.ToLower() == "model" || databaseName.ToLower() == "msdb" || databaseName.ToLower() == "tempdb")
            return true;
        return false;
    }
    private static bool IsSystemView(int viewId)
    {
        if (viewId < 0) return true;
        return false;
    }
    private static bool IsSystemStoredProc(int procId)
    {
        if (procId < 0) return true;
        return false;
    }
    #endregion
}