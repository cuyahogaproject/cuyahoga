<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Register.ascx.cs" Inherits="Cuyahoga.Web.Controls.Register" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h3>Register as new user</h3>
<asp:panel id="pnlRegister" runat="server">
	Please enter the desired username and your e-mail address. A generated password will be sent to the 
	e-mail address entered.
	<table>
		<tr>
			<td>Username</td>
			<td><asp:textbox id="txtUsername" runat="server" width="200px" maxlength="50"></asp:textbox></td>
		</tr>
		<tr>
			<td>Email</td>
			<td><asp:textbox id="txtEmail" runat="server" width="200px" maxlength="100"></asp:textbox></td>
		</tr>
		<tr>
			<td></td>
			<td><asp:button id="Button1" runat="server" text="Register"></asp:button></td>
		</tr>
	</table>
</asp:panel>
<asp:panel id="pnlConfirmation" runat="server" visible="False">
	Thank you for registering with this site. An e-mail with the password is sent to {email}.
</asp:panel>