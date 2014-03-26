<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Top.aspx.cs" Inherits="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body class="topPageBody">
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left" style="vertical-align: top;" class="topHeading">
                    <asp:Image ID="imgLogo" runat="server" Height="60px" />
                </td>
                <td align="right" style="vertical-align: top; width: 200px;" class="topHeading">
                    <img alt="" src="images/disconnect.gif" />
                    <asp:LinkButton ID="lnkDisconnect" runat="server" OnClick="lnkDisconnect_Click" CssClass="logOffButton">Disconnect</asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
