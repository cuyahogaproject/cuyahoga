<%@ Register TagPrefix="cc1" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<%@ Page language="c#" Codebehind="AdminArticles.aspx.cs" AutoEventWireup="True" Inherits="Cuyahoga.Modules.Articles.Web.AdminArticles" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>EditArticles</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Article management</h1>
				<p>
					<table class="tbl">
						<asp:repeater id="rptArticles" runat="server">
							<headertemplate>
								<tr>
									<th>
										Title</th>
									<th>
										Category</th>
									<th>
										Date online</th>
									<th>
										Date offline</th>
									<th>
										Created by</th>
									<th>
										Modified by</th>
									<th>
									</th>
								</tr>
							</headertemplate>
							<itemtemplate>
								<tr>
									<td><%# DataBinder.Eval(Container.DataItem, "Title") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "Category.Title") %></td>
									<td>
										<asp:literal id="litDateOnline" runat="server"></asp:literal></td>
									<td>
										<asp:literal id="litDateOffline" runat="server"></asp:literal></td>
									<td><%# DataBinder.Eval(Container.DataItem, "CreatedBy.Username") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "ModifiedBy.Username") %></td>
									<td>
										<asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink>
										<asp:hyperlink id="hplComments" runat="server">Comments</asp:hyperlink>
									</td>
								</tr>
							</itemtemplate>
						</asp:repeater>
					</table>
				</p>
				<div class="pager">
					<cc1:pager id="pgrArticles" runat="server" controltopage="rptArticles" cachedatasource="True"
						pagesize="10" cacheduration="30" cachevarybyparams="SectionId" oncacheempty="pgrArticles_CacheEmpty"></cc1:pager>
				</div>
				<br/>
				<input id="btnNew" type="button" value="New Article" runat="server">
			</div>
		</form>
	</body>
</html>
