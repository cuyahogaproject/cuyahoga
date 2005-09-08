<%@ Page language="c#" Codebehind="Modules.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.Modules" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Modules</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<table class="tbl">
				<asp:repeater id="rptModules" runat="server">
					<headertemplate>
						<tr>
							<th>
								Module name</th>
							<th>
								Assembly</th>
							<th>
								Status</th>
							<th>Actions</th>
						</tr>
					</headertemplate>
					<itemtemplate>
						<tr>
							<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
							<td><%# DataBinder.Eval(Container.DataItem, "AssemblyName") %></td>
							<td><asp:literal id="litStatus" runat="server"></asp:literal></td>
							<td>
								<asp:linkbutton id="lbtInstall" runat="server" visible="False" commandname="Install" commandargument='<%# DataBinder.Eval(Container.DataItem, "Name") + ":" + DataBinder.Eval(Container.DataItem, "AssemblyName") %>'>Install</asp:linkbutton>
								<asp:linkbutton id="lbtUpgrade" runat="server" visible="False" commandname="Upgrade" commandargument='<%# DataBinder.Eval(Container.DataItem, "Name") + ":" + DataBinder.Eval(Container.DataItem, "AssemblyName") %>'>Upgrade</asp:linkbutton>
								<asp:linkbutton id="lbtUninstall" runat="server" visible="False" commandname="Uninstall" commandargument='<%# DataBinder.Eval(Container.DataItem, "Name") + ":" + DataBinder.Eval(Container.DataItem, "AssemblyName") %>'>Uninstall</asp:linkbutton>
							</td>
						</tr>
					</itemtemplate>
				</asp:repeater>
			</table>
		</form>
	</body>
</html>
