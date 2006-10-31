<%@ Page language="c#" Codebehind="Roles.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.Roles" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Roles</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">&nbsp;
			<table class="tbl">
				<asp:repeater id="rptRoles" runat="server">
					<headertemplate>
						<tr>
						    <th></th>
							<th>Rolename</th>
							<th>Permissionlevel(s)</th>
							<th>Last update</th>
							<th></th>
						</tr>
					</headertemplate>
					<itemtemplate>
						<tr>
						    <td><asp:Image Width="14" Height="12" ImageAlign="Middle" runat="server" ID="imgRole" /></td>
							<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
							<td><asp:label id="lblPermissions" runat="server"></asp:label></td>
							<td><asp:label id="lblLastUpdate" runat="server"></asp:label></td>
							<td>
								<asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink>
							</td>
						</tr>
					</itemtemplate>
				</asp:repeater>
			</table>
			<br/>
			<div>
				<asp:button id="btnNew" runat="server" text="Add new role"></asp:button>
			</div>
		</form>
	</body>
</html>
