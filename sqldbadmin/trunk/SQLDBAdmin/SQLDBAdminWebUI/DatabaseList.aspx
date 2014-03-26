<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DatabaseList.aspx.cs" Inherits="DatabaseList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <script language="javascript" type="text/javascript">
    function OnExpandClick(evt) 
    {
        var evt = evt || window.event; // event object
        var obj = evt.target || window.event.srcElement; // event target        
        if (obj.tagName.toLowerCase() == "img")
        {
            if(obj.alt.toLowerCase().substring(0, 6) == "expand")
            {
                SetImage(obj);
            }                        
        }
        else if(obj.tagName.toLowerCase() == "a")
            {
                var nodePrefx = obj.id.substring(0,obj.id.lastIndexOf('t'));
                var nodeSufx = obj.id.substring(obj.id.lastIndexOf('t'));
                var imgHolderId = nodePrefx + nodeSufx.replace("t","n");
                var imgToggle = document.getElementById(imgHolderId).getElementsByTagName("img")[0];
                SetImage(imgToggle);
            }
    } 
    function SetImage(obj)
    {
        obj.src = "images/loading.gif";
        obj.alt = "Loading ...";           
    }
    
    </script>

    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>       
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td align="left">                    
                    <asp:TreeView ID="treeViewDBList" runat="server" CssClass="treeview" OnTreeNodePopulate="PopulateNode"
                        PopulateNodesFromClient="true" onclick="OnExpandClick(event);">
                        <NodeStyle CssClass="treeviewItem" />
                    </asp:TreeView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
