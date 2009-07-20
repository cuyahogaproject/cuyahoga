<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Category>>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.ManageCategoriesPageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TasksLabel %></h2>
	<%= Html.ActionLink(GlobalResources.CreateCategoryLabel, "New", "Categories", null, new { @class = "createlink" }) %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= GlobalResources.ManageCategoriesPageTitle %></h1>
	<table class="grid" style="width:100%">
		<thead>
			<tr>
				<th><%= GlobalResources.NameLabel %></th>
				<th><%= GlobalResources.DescriptionLabel %></th>
				<th>&nbsp;</th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var category in Model) { %>
				<tr>
					<td style="padding-left:<%= category.Level * 20 %>px"><%= category.Name %></td>
					<td><%= category.Description %></td>
					<td>
						<%= Html.ActionLink(GlobalResources.EditLabel, "Edit", new { id = category.Id }) %>
						<% using (Html.BeginForm("Delete", "Categories", new { id = category.Id }, FormMethod.Post)) { %>
							<a href="#" class="deletelink"><%= GlobalResources.DeleteButtonLabel %></a>
						<% } %>
					</td>
				</tr>
			<% } %>
		</tbody>
	</table>
</asp:Content>
