<%@ Page language="c#" Codebehind="RoleEdit.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.RoleEdit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
  <head>
		<title>RoleEdit</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
	<body ms_positioning="FlowLayout">

		<form id="Form1" method="post" runat="server">
			<div class="group">
				<h4>General</h4>
				<table>
					<tr>
						<td style="WIDTH: 200px">Name</td>
						<td><asp:textbox id="txtName" runat="server" width="200px"></asp:textbox>
							<asp:requiredfieldvalidator id="rfvName" runat="server" errormessage="Name is required" cssclass="validator" display="Dynamic" enableclientscript="False" controltovalidate="txtName"></asp:requiredfieldvalidator>
						</td>
					</tr>
					<tr>
						<td>Permissions</td>
						<td><asp:checkboxlist id="cblRoles" runat="server" repeatlayout="Flow"></asp:checkboxlist>

						</td>
					</tr>
				</table>
			</div>
			<br/>
			<asp:button id="btnSave" runat="server" text="Save"></asp:button>
			<asp:Button id="btnCancel" runat="server" Text="Cancel" causesvalidation="false"></asp:Button>
			<asp:Button id="btnDelete" runat="server" Text="Delete" causesvalidation="false"></asp:Button>
		</form>

	</body>
</html>
