<%@ Page language="c#" Codebehind="Templates.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.Templates" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 TRANSITIONAL//EN" >
<html>
	<head>
		<title>Templates</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<p><em>Note that you have to maintain
					the physical templates separately with FTP or something else...</em> </p>
			<table class="tbl"><asp:repeater id="rptTemplates" runat="server">
					<headertemplate>
						<tr>
							<th>Template name</th>
							<th>Path</th>
							<th>Css</th>
							<th></th>
						</tr>
					</headertemplate>
					<itemtemplate>
						<tr>
							<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
							<td><%# DataBinder.Eval(Container.DataItem, "Path") %></td>
							<td><%# DataBinder.Eval(Container.DataItem, "Css") %></td>
							<td>
								<asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink>
							</td>
						</tr>
					</itemtemplate>
				</asp:repeater></table><br>
			<div>
				<asp:button id="btnNew" runat="server" text="Add new template"></asp:button>
			</div></form>
	</body>
</html>
