<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Users.Roles" %>
<%@ Import Namespace="Cuyahoga.Web.Mvc.Helpers"%>
<%@ Import Namespace="Cuyahoga.Core.Service.Membership"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TasksLabel %></h2>
	<ul>
		<li><%= Html.ActionLink(GlobalResources.CreateRoleLabel, "NewRole", "Users") %></li>
	</ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
		<% if (ViewData.Model.Count > 0) { %>
		<table class="grid" style="width:100%">
			<thead>
				<tr>
					<th><%= GlobalResources.RoleLabel %></th>
					<th><%= GlobalResources.RightsLabel %></th>
					<th><%= GlobalResources.IsGlobalLabel%></th>
					<th>&nbsp;</th>
				</tr>
			</thead>
			<tbody>
				<% foreach (var role in ViewData.Model) { %>
					<tr>
						<td><%= role.Name %></td>
						<td><%= role.RightsString %></td>	
						<td><%= role.IsGlobal %></td>
						<td>
							<% if (! role.IsGlobal || Html.HasRight(User, Rights.GlobalPermissions)) { %>
								<%= Html.ActionLink(GlobalResources.EditLabel, "EditRole", new { id = role.Id }) %>
								<% using (Html.BeginForm("DeleteRole", "Users", new { id = role.Id }, FormMethod.Post)) { %>
									<a href="#" class="deletelink"><%= GlobalResources.DeleteButtonLabel %></a>
								<% } %>
							<% } %>
						</td>
					</tr>
				<% } %>
			</tbody>
		</table>
	<% } else { %>
		<%= GlobalResources.NoRolesFound %>
	<% } %>
</asp:Content>
