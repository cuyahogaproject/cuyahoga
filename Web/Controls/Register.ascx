<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Register.ascx.cs" Inherits="Cuyahoga.Web.Controls.Register" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h3><%= GetText("REGISTERTITLE") %></h3>
<br/>
<asp:label id="lblError" runat="server" visible="False" cssclass="error" enableviewstate="False"></asp:label>
<br/>
<asp:panel id="pnlRegister" runat="server">
	<%= GetText("REGISTERINFO") %><br/>
	<table>
		<tr>
			<td><%= GetText("USERNAME") %></td>
			<td><asp:textbox id="txtUsername" runat="server" maxlength="50" width="200px"></asp:textbox><asp:requiredfieldvalidator id=rfvUsername runat="server" ErrorMessage='<%# GetText("USERNAMEREQUIRED") %>' display="Dynamic" cssclass="error" controltovalidate="txtUsername" enableclientscript="False"></asp:requiredfieldvalidator></td></tr>
		<tr>
			<td><%= GetText("EMAIL") %></td>
			<td><asp:textbox id="txtEmail" runat="server" maxlength="100" width="200px"></asp:textbox><asp:requiredfieldvalidator id=rfvEmail runat="server" ErrorMessage='<%# GetText("EMAILREQUIRED") %>' display="Dynamic" cssclass="error" controltovalidate="txtEmail" enableclientscript="False"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id=revEmail runat="server" ErrorMessage='<%# GetText("EMAILINVALID") %>' display="Dynamic" cssclass="error" controltovalidate="txtEmail" enableclientscript="False" validationexpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:regularexpressionvalidator></td></tr>
		<tr>
			<td></td>
			<td><asp:button id=btnRegister runat="server" Text='<%# GetText("REGISTER") %>'></asp:button></td></tr></table>
</asp:panel>
<asp:panel id="pnlConfirmation" runat="server" visible="False">
	<asp:label id="lblConfirmation" runat="server"></asp:label>
</asp:panel>
