<%@ Page language="c#" Codebehind="Users.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.Users" %>
<%@ Register TagPrefix="csc" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
  <head>
		<title>Users</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
	<body ms_positioning="FlowLayout">

		<form id="Form1" method="post" runat="server">
			<div class="group">
				<h4>Find users</h4>
				Username
				<asp:textbox id="txtUsername" runat="server"></asp:textbox>
				<asp:button id="btnFind" runat="server" text="Find"></asp:button>
			</div>
			<asp:panel id="pnlResults" runat="server" visible="False" cssclass="group">
				<h4>Search results</h4>
				<p>
				<table class="tbl">
				<asp:Repeater id=rptUsers runat="server">
					<headertemplate>
						<tr>
							<th>Username</th>
							<th>Firstname</th>
							<th>Lastname</th>
							<th>Email</th>
						</tr>
					</headertemplate>
					<itemtemplate>
						<tr>
							<td><%# DataBinder.Eval(Container.DataItem, "UserName") %></td>
							<td><%# DataBinder.Eval(Container.DataItem, "FirstName") %></td>
							<td><%# DataBinder.Eval(Container.DataItem, "LastName") %></td>
							<td><%# DataBinder.Eval(Container.DataItem, "Email") %></td>
						</tr>
					</itemtemplate>
				</asp:Repeater>
				</table>
				</p>
				<p><csc:Pager id=pgrUsers runat="server"></csc:Pager></p>
			</asp:panel>
		</form>

	</body>
</html>
