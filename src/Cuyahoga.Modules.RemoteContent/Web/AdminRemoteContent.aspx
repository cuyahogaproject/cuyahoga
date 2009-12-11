<%@ Page language="c#" Codebehind="AdminRemoteContent.aspx.cs" AutoEventWireup="True" Inherits="Cuyahoga.Modules.RemoteContent.Web.AdminRemoteContent" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>AdminRemoteContent</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Remote content management</h1>
				<p>
					Note that you can combine multiple feeds in one display.
					<table class="tbl">
						<asp:repeater id="rptFeeds" runat="server">
							<headertemplate>
								<tr>
									<th>
										Title</th>
									<th>
										url</th>
									<th>
										Number of items to display</th>
									<th>
										Feed publication date</th>
									<th>
										Last updated</th>
									<th>
									</th>
								</tr>
							</headertemplate>
							<itemtemplate>
								<tr>
									<td><%# DataBinder.Eval(Container.DataItem, "Title") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "Url") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "NumberOfItems") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "PubDate") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "UpdateTimestamp") %></td>
									<td>
										<asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink>
										<asp:linkbutton id="lbtRefresh" runat="server" causesvalidation="False" commandname="Refresh" commandargument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>Refresh</asp:linkbutton>
									</td>
								</tr>
							</itemtemplate>
						</asp:repeater>
					</table>
				</p>
				<br/>
				<input id="btnNew" type="button" value="Add Feed" runat="server" name="btnNew">
			</div>
		</form>
	</body>
</html>
