<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ResetPassword.ascx.cs" Inherits="Cuyahoga.Web.Controls.ResetPassword" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h3><%= GetTextFromFile("RESETTITLE") %></h3>
<br/>
<asp:panel id="pnlReset" runat="server">
	<%= GetTextFromFile("RESETINFO") %><br/>
	<asp:label id="lblError" runat="server" cssclass="error" enableviewstate="False" visible="False"></asp:label>
	<table>
		<tr>
			<td><%= GetTextFromFile("USERNAME") %></td>
			<td><asp:textbox id="txtUsername" runat="server" maxlength="50" width="200px"></asp:textbox><asp:requiredfieldvalidator id=rfvUsername runat="server" cssclass="error" enableclientscript="False" controltovalidate="txtUsername" display="Dynamic" errormessage='<%# GetTextFromFile("USERNAMEREQUIRED") %>'></asp:requiredfieldvalidator></td></tr>
		<tr>
			<td><%= GetTextFromFile("EMAIL") %></td>
			<td><asp:textbox id="txtEmail" runat="server" maxlength="100" width="200px"></asp:textbox><asp:requiredfieldvalidator id=rfvEmail runat="server" cssclass="error" enableclientscript="False" controltovalidate="txtEmail" display="Dynamic" errormessage='<%# GetTextFromFile("EMAILREQUIRED") %>'></asp:requiredfieldvalidator><asp:regularexpressionvalidator id=revEmail runat="server" cssclass="error" enableclientscript="False" controltovalidate="txtEmail" display="Dynamic" errormessage='<%# GetTextFromFile("EMAILINVALID") %>' validationexpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:regularexpressionvalidator></td></tr>
		<tr>
			<td></td>
			<td><asp:button id=btnReset runat="server" text='<%# GetTextFromFile("RESET") %>'></asp:button></td></tr></table>
</asp:panel>
<asp:panel id="pnlConfirmation" runat="server" visible="False"><asp:label id="lblConfirmation" runat="server"></asp:label>
</asp:panel>
