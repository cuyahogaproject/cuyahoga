<%@ Register TagPrefix="csc" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<%@ Page language="c#" Codebehind="Users.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.Users" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
  <head>
		<title>Users</title>
<meta content="Microsoft Visual Studio .NET 7.1" name=GENERATOR>
<meta content=C# name=CODE_LANGUAGE>
<meta content=JavaScript name=vs_defaultClientScript>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema>
  </head>
<body ms_positioning="FlowLayout">
<form id=Form1 method=post runat="server">
<div class=group>
<h4>Find users</H4>Username <asp:textbox id=txtUsername runat="server"></asp:textbox><asp:button id=btnFind runat="server" text="Find"></asp:button></DIV><asp:panel 
id=pnlResults runat="server" cssclass="group">
<h4>Search results</h4>
<p>
<table class=tbl><asp:repeater id=rptUsers runat="server">
							<itemtemplate>
								<tr>
									<td><%# DataBinder.Eval(Container.DataItem, "UserName") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "FirstName") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "LastName") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "Email") %></td>
								</tr>

							</itemtemplate>

							<headertemplate>
								<tr>
									<th>Username</th>
									<th>Firstname</th>
									<th>Lastname</th>
									<th>Email</th>
								</tr>

							</headertemplate>
						</asp:repeater></table></p>
<div class=pager><csc:pager id=pgrUsers runat="server" controltopage="rptUsers" BackColor="#FFE0C0" PageSize="20"></csc:pager></div></asp:panel></FORM>

	</body>
</html>
