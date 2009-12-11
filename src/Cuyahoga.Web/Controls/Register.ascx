<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Register.ascx.cs" Inherits="Cuyahoga.Web.Controls.Register" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h3><%= GetTextFromFile("REGISTERTITLE") %></h3>
<br/>
<asp:label id="lblError" runat="server" visible="False" cssclass="error" enableviewstate="False"></asp:label>
<br/>
<asp:panel id="pnlRegister" runat="server">
	<%= GetTextFromFile("REGISTERINFO") %><br/>
	<table>
		<tr>
			<td><%= GetTextFromFile("USERNAME") %></td>
			<td><asp:textbox id="txtUsername" runat="server" maxlength="50" width="200px"></asp:textbox><asp:requiredfieldvalidator id=rfvUsername runat="server" ErrorMessage='<%# GetTextFromFile("USERNAMEREQUIRED") %>' display="Dynamic" cssclass="error" controltovalidate="txtUsername" enableclientscript="False"></asp:requiredfieldvalidator></td></tr>
		<tr>
			<td><%= GetTextFromFile("EMAIL") %></td>
			<td><asp:textbox id="txtEmail" runat="server" maxlength="100" width="200px"></asp:textbox><asp:requiredfieldvalidator id=rfvEmail runat="server" ErrorMessage='<%# GetTextFromFile("EMAILREQUIRED") %>' display="Dynamic" cssclass="error" controltovalidate="txtEmail" enableclientscript="False"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id=revEmail runat="server" ErrorMessage='<%# GetTextFromFile("EMAILINVALID") %>' display="Dynamic" cssclass="error" controltovalidate="txtEmail" enableclientscript="False" validationexpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:regularexpressionvalidator></td></tr>
		<tr>
			<td></td>
			<td><asp:button id=btnRegister runat="server" Text='<%# GetTextFromFile("REGISTER") %>'></asp:button></td></tr></table>
</asp:panel>
<asp:panel id="pnlConfirmation" runat="server" visible="False">
	<asp:label id="lblConfirmation" runat="server"></asp:label>
</asp:panel>
