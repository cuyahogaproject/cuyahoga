<%@ Page language="c#" Codebehind="Login.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.Login" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Login</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" type="text/css" href="Admin/Css/Admin.css">
	</head>
	<body ms_positioning="FlowLayout">

		<form id="Form1" method="post" runat="server">
			<div id="login">
				<h3>Cuyahoga login</h3>
				<table width="100%">
					<tr>
						<td></td>
						<td><asp:label id="lblError" runat="server" enableviewstate="False" visible="False" cssclass="validator"></asp:label></td></tr>
					<tr>
						<td style="width:90px">User</td>
						<td><asp:textbox id="txtUsername" runat="server" width="140px"></asp:textbox></td>
					</tr>
					<tr>
						<td>Password</td>
						<td><asp:textbox id="txtPassword" runat="server" textmode="Password" width="140px"></asp:textbox></td>
					</tr>
					<tr>
						<td></td>
						<td><asp:button id="btnLogin" runat="server" text="Login"></asp:button></td>
					</tr>
				</table>
			</div>
		</form>

	</body>
</html>
