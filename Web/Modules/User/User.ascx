<%@ Control Language="c#" AutoEventWireup="false" Codebehind="User.ascx.cs" Inherits="Cuyahoga.Web.Modules.User.User" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:Panel id=pnlLogin runat="server">
	Username<br><asp:TextBox id=txtUsername runat="server" width="120px"></asp:TextBox><br>Password<br><asp:TextBox id=txtPassword runat="server" TextMode="Password" width="120px"></asp:TextBox><br><asp:label id=lblLoginError runat="server" enableviewstate="False" visible="False" cssclass="error"></asp:label><br><asp:Button id=btnLogin runat="server" Text="Login"></asp:Button>
</asp:Panel>
<asp:Panel id=pnlUserInfo runat="server" Visible="False">
	Logged in as <asp:Label id=lblUsername runat="server"></asp:Label><br><asp:Button id=btnLogout runat="server" Text="Logout"></asp:Button>
</asp:Panel>
