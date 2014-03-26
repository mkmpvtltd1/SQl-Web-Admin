<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Query.aspx.cs" Inherits="Query" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>

    <script language="javascript" type="text/javascript">
    function resizeFrame()
    {
        var frameSet = parent.document.getElementById('main_frameset');
        var image = document.getElementById('imgToggle');
        var hiddenFieldUrl = document.getElementById("<%=hiddenImageToggleUrl.ClientID %>");
        var hiddenFieldToolTip = document.getElementById("<%=hiddenImageToggleToolTip.ClientID %>");        
        if(frameSet.reduced)
        {
            frameSet.setAttribute("cols","350,*");    
            image.src = "images/reducenavbar.gif";
            image.title = "Expand";
            hiddenFieldUrl.value = "~/images/reducenavbar.gif";
            hiddenFieldToolTip.value = "Expand";
        }
        else
        {    
            frameSet.setAttribute("cols","0,*");            
            image.src = "images/opennavbar.gif";
            image.title = "Collapse";
            hiddenFieldUrl.value = "~/images/opennavbar.gif";            
            hiddenFieldToolTip.value = "Collapse";
        }
        frameSet.reduced=!(frameSet.reduced);        
    }
    </script>

    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:HiddenField ID="hiddenImageToggleUrl" runat="server" />
        <asp:HiddenField ID="hiddenImageToggleToolTip" runat="server" />
        <asp:Panel ID="pnlPageContainer" runat="server" CssClass="queryPageContainer">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td align="left">
                        <b>Database : </b>
                        <asp:DropDownList ID="ddlDatabases" runat="server" CssClass="dropdownstyle">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td height="10">
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td align="left" style="width: 1px;">
                                    <asp:Image ID="imgToggle" runat="server" CssClass="imgToggle" onclick="resizeFrame(); return false;" />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtQuery" runat="server" CssClass="textQuery" TextMode="MultiLine"></asp:TextBox>
                                    <cc1:TextBoxWatermarkExtender ID="tbwe" runat="server" TargetControlID="txtQuery"
                                        WatermarkCssClass="waterMark" WatermarkText="... Write your Queries here ...">
                                    </cc1:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="20">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Button ID="btnSubmit" runat="server" Text="! Execute" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                        <asp:Button ID="btnSave" runat="server" Text="Save Query" OnClick="btnSave_Click" />
                        <asp:Button ID="btnOpen" runat="server" Text="Open Query" />
                    </td>
                </tr>
                <tr>
                    <td height="20">
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlResult" runat="server" Width="100%">
                <asp:Label ID="lblError" runat="server" CssClass="errorMsg"></asp:Label>
            </asp:Panel>
        </asp:Panel>
        <cc1:ModalPopupExtender ID="MPE" runat="server" TargetControlID="btnOpen" PopupControlID="popFileOpen"
            CancelControlID="btnCancelWriteFile" BackgroundCssClass="modalPopupBackground" />
        <asp:Panel ID="popFileOpen" runat="server" Style="display: none;">
            <asp:FileUpload ID="fileUploadQuery" runat="server" CssClass="fileUpload" />
            <br />
            <asp:Button ID="btnWriteFile" runat="server" Text="Done" OnClick="btnWriteFile_Click" />
            <asp:Button ID="btnCancelWriteFile" runat="server" Text="Cancel" />
        </asp:Panel>
    </div>
    </form>
</body>
</html>
