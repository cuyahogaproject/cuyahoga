<%@ Control Language="c#" AutoEventWireup="True" Codebehind="User.ascx.cs" Inherits="Cuyahoga.Modules.User.User" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:panel id="pnlLogin" runat="server">
<asp:label id="lblUsername" runat="server"></asp:label><br/>
<asp:textbox id="txtUsername" runat="server" width="120px"></asp:textbox><br/>
<asp:label id="lblPassword" runat="server"></asp:label><br/>
<asp:textbox id="txtPassword" runat="server" width="120px" textmode="Password"></asp:textbox><br/>
<asp:checkbox id="chkPersistLogin" runat="server"></asp:checkbox><br/>
<asp:label id="lblLoginError" runat="server" enableviewstate="False" visible="False" cssclass="error"></asp:label><br/>
<asp:button id="btnLogin" runat="server" onclick="btnLogin_Click"></asp:button><br/><br/>
<asp:hyperlink id="hplRegister" runat="server"></asp:hyperlink>&nbsp;&nbsp; 
<asp:hyperlink id="hplResetPassword" runat="server"></asp:hyperlink>
</asp:panel>
<asp:panel id="pnlUserInfo" runat="server" visible="False">
	<asp:label id="lblLoggedInText" runat="server"></asp:label>
	<asp:label id="lblLoggedInUser" runat="server"></asp:label>
	<br/>
	<asp:button id="btnLogout" runat="server" onclick="btnLogout_Click"></asp:button>
	<br/>
	<br/>
	<asp:hyperlink id="hplEdit" runat="server"></asp:hyperlink>
</asp:panel>
