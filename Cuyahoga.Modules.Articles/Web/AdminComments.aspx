<%@ Page language="c#" Codebehind="AdminComments.aspx.cs" AutoEventWireup="True" Inherits="Cuyahoga.Modules.Articles.Web.AdminComments" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Edit comments</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body>

		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Article comments</h1>
				<p>
					<table class="tbl">
						<asp:repeater id="rptComments" runat="server">
							<headertemplate>
								<tr>
									<th>Comment</th>
									<th>From</th>
									<th>IP</th>
									<th>Date</th>
									<th></th>
								</tr>
							</headertemplate>
							<itemtemplate>
								<tr>
									<td><%# DataBinder.Eval(Container.DataItem, "CommentText") %></td>
									<td><asp:literal id="litFrom" runat="server"></asp:literal></td>
									<td><%# DataBinder.Eval(Container.DataItem, "UserIp") %></td>
									<td><asp:literal id="litUpdateTimestamp" runat="server"></asp:literal></td>
									<td><asp:linkbutton id="lbtDelete" runat="server">Delete</asp:linkbutton></td>
								</tr>
							</itemtemplate>
						</asp:repeater>
					</table>
				</p>
				<br/>
				<asp:button id="btnBack" runat="server" text="Back" onclick="btnBack_Click"></asp:button>
			</div>
		</form>

	</body>
</html>
