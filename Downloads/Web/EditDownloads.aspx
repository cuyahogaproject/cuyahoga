<%@ Page language="c#" Codebehind="EditDownloads.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Downloads.Web.EditDownloads" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>EditDownloads</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Downloads management</h1>
				<p>
					<table class="tbl">
						<asp:repeater id="rptFiles" runat="server">
							<headertemplate>
								<tr>
									<th>
										Title</th>
									<th>
										Filename</th>
									<th>
										Size (bytes)</th>
									<th>
										Published by</th>
									<th>
										Number of downloads</th>
									<th>
										Date published</th>
									<th>
									</th>
								</tr>
							</headertemplate>
							<itemtemplate>
								<tr>
									<td><%# DataBinder.Eval(Container.DataItem, "Title") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "FilePath") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "Size") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "Publisher.FullName") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "NrOfDownloads") %></td>
									<td><asp:literal id="litDateModified" runat="server"></asp:literal></td>
									<td>
										<asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink>
									</td>
								</tr>
							</itemtemplate>
						</asp:repeater>
					</table>
				</p>
				<br/>
				<input id="btnNew" type="button" value="Add File" runat="server" name="btnNew">
			</div>
		</form>
	</body>
</html>
