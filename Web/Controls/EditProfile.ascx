<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditProfile.ascx.cs" Inherits="Cuyahoga.Web.Controls.EditProfile" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h3><%= GetTextFromFile("EDITPROFILETITLE") %></h3>
<br/>
<asp:panel id="pnlEdit" runat="server">
	<%= GetTextFromFile("EDITPROFILEINFO") %>
	<br/>
	<asp:label id="lblError" runat="server" visible="False" cssclass="error" enableviewstate="False"></asp:label>
	<br/>
	<table class="tbl">
		<tr>
			<td style="WIDTH: 200px"><%= GetTextFromFile("USERNAME") %></td>
			<td class="tblvalue">
				<asp:label id="lblUsername" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td><%= GetTextFromFile("FIRSTNAME") %></td>
			<td>
				<asp:textbox id="txtFirstname" runat="server" maxlength="100" width="200px"></asp:textbox></td>
		</tr>
		<tr>
			<td><%= GetTextFromFile("LASTNAME") %></td>
			<td>
				<asp:textbox id="txtLastname" runat="server" maxlength="100" width="200px"></asp:textbox></td>
		<tr>
			<td><%= GetTextFromFile("EMAIL") %></td>
			<td>
				<asp:textbox id="txtEmail" runat="server" maxlength="100" width="200px"></asp:textbox>
				<asp:requiredfieldvalidator id=rfvEmail runat="server" cssclass="error" errormessage='<%# GetTextFromFile("EMAILREQUIRED") %>' display="Dynamic" controltovalidate="txtEmail" enableclientscript="False">
				</asp:requiredfieldvalidator>
				<asp:regularexpressionvalidator id=revEmail runat="server" cssclass="error" errormessage='<%# GetTextFromFile("EMAILINVALID") %>' display="Dynamic" controltovalidate="txtEmail" enableclientscript="False" validationexpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
				</asp:regularexpressionvalidator></td>
		<tr>
			<td><%= GetTextFromFile("WEBSITE") %></td>
			<td>
				<asp:textbox id="txtWebsite" runat="server" maxlength="100" width="200px"></asp:textbox></td>
		<tr>
			<td><%= GetTextFromFile("TIMEZONE") %></td>
			<td>
				<asp:dropdownlist id="ddlTimeZone" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td></td>
			<td>
				<asp:button id=btnSave runat="server" text='<%# GetTextFromFile("SAVEPROFILE") %>'>
				</asp:button></td>
		</tr>
	</table>
	<br/>
	<table class="tbl">
		<tr>
			<td><%= GetTextFromFile("CURRENTPASSWORD") %></td>
			<td>
				<asp:textbox id="txtCurrentPassword" runat="server" maxlength="100" width="100px" textmode="Password"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 200px"><%= GetTextFromFile("NEWPASSWORD") %></td>
			<td>
				<asp:textbox id="txtNewPassword" runat="server" maxlength="100" width="100px" textmode="Password"></asp:textbox></td>
		</tr>
		</TR>
		<tr>
			<td><%= GetTextFromFile("NEWPASSWORDCONFIRMATION") %></td>
			<td>
				<asp:textbox id="txtNewPasswordConfirmation" runat="server" maxlength="100" width="100px" textmode="Password"></asp:textbox></td>
		</tr>
		</TR>
		<tr>
			<td></td>
			<td>
				<asp:button id=btnSavePassword runat="server" text='<%# GetTextFromFile("SAVEPASSWORD") %>'>
				</asp:button></td>
		</tr>
	</table>
</asp:panel>
<asp:panel id="pnlInfo" runat="server" visible="False">
	<asp:label id="lblInfo" runat="server"></asp:label>
</asp:panel>
