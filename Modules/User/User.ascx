<%@ Control Language="c#" AutoEventWireup="false" Codebehind="User.ascx.cs" Inherits="Cuyahoga.Modules.User.User" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:panel id="pnlLogin" runat="server"><asp:label id="lblUsername" runat="server"></asp:label><br><asp:textbox id="txtUsername" runat="server" width="120px"></asp:textbox><br><asp:label id="lblPassword" runat="server"></asp:label><br><asp:textbox id="txtPassword" runat="server" width="120px" textmode="Password"></asp:textbox><br><asp:label id="lblLoginError" runat="server" cssclass="error" visible="False" enableviewstate="False"></asp:label><br><asp:button id="btnLogin" runat="server"></asp:button>
</asp:panel>
<asp:panel id="pnlUserInfo" runat="server" visible="False"><asp:label id="lblLoggedInText" runat="server"></asp:label>&nbsp; <asp:label id="lblLoggedInUser" runat="server"></asp:label><br><asp:button id="btnLogout" runat="server"></asp:button>
</asp:panel>
