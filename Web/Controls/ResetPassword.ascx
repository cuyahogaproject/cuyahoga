<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ResetPassword.ascx.cs" Inherits="Cuyahoga.Web.Controls.ResetPassword" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h3>Reset password</h3>
<asp:panel id="pnlReset" runat="server">
	Please enter your username and your e-mail address. A new password will be sent to you.
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
			<td><asp:button id="Button1" runat="server" text="Reset password"></asp:button></td>
		</tr>
	</table>
</asp:panel>
<asp:panel id="pnlConfirmation" runat="server" visible="False">
	Your password has been reset. You will be getting an e-mail with your new password.
</asp:panel>