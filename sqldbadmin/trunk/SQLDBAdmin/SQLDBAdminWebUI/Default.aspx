<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DB Admin - Login</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td class="loginVerticalSpace">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="updatePanelLogin" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnLogin" BorderColor="Gray"
                                BorderWidth="1px" BorderStyle="Groove" Width="400px">
                                <table cellpadding="0" cellspacing="0" border="0" class="loginDiv">
                                    <tr>
                                        <td class="space" colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 20px" colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Server:
                                        </td>
                                        <td align="left" class="formPadLeft">
                                            <asp:DropDownList ID="ddlServers" runat="server" Width="205px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="space" colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Database:
                                        </td>
                                        <td align="left" class="formPadLeft">
                                            <asp:TextBox ID="txtDatabase" runat="server" CssClass="textBoxLogin"></asp:TextBox>
                                            <i>(optional)</i>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="space" colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            User Name:
                                        </td>
                                        <td align="left" class="formPadLeft">
                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="textBoxLogin"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="User Name is required"
                                                SetFocusOnError="true" ControlToValidate="txtUserName" Display="None">
                                            </asp:RequiredFieldValidator>
                                            <cc1:ValidatorCalloutExtender ID="vceUserName" runat="server" TargetControlID="rfvUserName">
                                            </cc1:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="space" colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Password:
                                        </td>
                                        <td align="left" class="formPadLeft">
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="textBoxLogin"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password is required"
                                                Display="None" ControlToValidate="txtPassword" SetFocusOnError="true">
                                            </asp:RequiredFieldValidator>
                                            <cc1:ValidatorCalloutExtender ID="vcePassword" runat="server" TargetControlID="rfvPassword">
                                            </cc1:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="space" colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="left" class="formPadLeft">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:UpdateProgress runat="server" ID="progress" AssociatedUpdatePanelID="updatePanelLogin">
                                                            <ProgressTemplate>
                                                                <img alt="Please wait..." src="images/loading.gif" />
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 20px" align="center">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="errorMsg"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="space" colspan="2">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
