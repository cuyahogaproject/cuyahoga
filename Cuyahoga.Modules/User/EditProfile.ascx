<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditProfile.ascx.cs" Inherits="Cuyahoga.Modules.User.EditProfile" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<h3><%= GetText("EDITPROFILETITLE") %></h3>
<br/>
<asp:panel id="pnlEdit" runat="server">
	<%= GetText("EDITPROFILEINFO") %>
	<br/>
	<asp:label id="lblError" runat="server" visible="False" cssclass="error" enableviewstate="False"></asp:label>
	<br/>
	<table class="tbl">
		<tr>
			<td style="WIDTH: 200px"><%= GetText("USERNAME") %></td>
			<td class="tblvalue">
				<asp:label id="lblUsername" runat="server"></asp:label></td>
		</tr>
		<tr>
			<td><%= GetText("FIRSTNAME") %></td>
			<td>
				<asp:textbox id="txtFirstname" runat="server" maxlength="100" width="200px"></asp:textbox></td>
		</tr>
		<tr>
			<td><%= GetText("LASTNAME") %></td>
			<td>
				<asp:textbox id="txtLastname" runat="server" maxlength="100" width="200px"></asp:textbox></td>
		</tr>
		<tr>
			<td><%= GetText("EMAIL") %></td>
			<td>
				<asp:textbox id="txtEmail" runat="server" maxlength="100" width="200px"></asp:textbox>
				<asp:requiredfieldvalidator id="rfvEmail" runat="server" cssclass="error" errormessage='<%# GetText("EMAILREQUIRED") %>' display="Dynamic" controltovalidate="txtEmail" enableclientscript="False">
				</asp:requiredfieldvalidator>
				<asp:regularexpressionvalidator id="revEmail" runat="server" cssclass="error" errormessage='<%# GetText("EMAILINVALID") %>' display="Dynamic" controltovalidate="txtEmail" enableclientscript="False" validationexpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
				</asp:regularexpressionvalidator></td>
		</tr>
		<tr>
			<td><%= GetText("WEBSITE") %></td>
			<td>
				<asp:textbox id="txtWebsite" runat="server" maxlength="100" width="200px"></asp:textbox></td>
		</tr>
		<tr>
			<td><%= GetText("TIMEZONE") %></td>
			<td>
				<asp:dropdownlist id="ddlTimeZone" runat="server"></asp:dropdownlist></td>
		</tr>
		<tr>
			<td></td>
			<td>
				<asp:button id=btnSave runat="server" text='<%# GetText("SAVEPROFILE") %>' onclick="btnSave_Click">
				</asp:button></td>
		</tr>
	</table>
	<br/>
	<table class="tbl">
		<tr>
			<td><%= GetText("CURRENTPASSWORD") %></td>
			<td>
				<asp:textbox id="txtCurrentPassword" runat="server" maxlength="100" width="100px" textmode="Password"></asp:textbox></td>
		</tr>
		<tr>
			<td style="WIDTH: 200px"><%= GetText("NEWPASSWORD") %></td>
			<td>
				<asp:textbox id="txtNewPassword" runat="server" maxlength="100" width="100px" textmode="Password"></asp:textbox></td>
		</tr>
		<tr>
			<td><%= GetText("NEWPASSWORDCONFIRMATION") %></td>
			<td>
				<asp:textbox id="txtNewPasswordConfirmation" runat="server" maxlength="100" width="100px" textmode="Password"></asp:textbox></td>
		</tr>
		<tr>
			<td></td>
			<td>
				<asp:button id="btnSavePassword" runat="server" text='<%# GetText("SAVEPASSWORD") %>' onclick="btnSavePassword_Click">
				</asp:button></td>
		</tr>
	</table>
</asp:panel>
<asp:panel id="pnlInfo" runat="server" visible="False">
	<asp:label id="lblInfo" runat="server"></asp:label>
</asp:panel>
