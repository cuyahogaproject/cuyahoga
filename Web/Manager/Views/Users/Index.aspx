<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Users.Index" %>
<%@ Import Namespace="Cuyahoga.Web.Mvc.Paging"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TasksLabel %></h2>
	<ul>
		<li><%= GlobalResources.CreateUserLabel %></li>
		<li><%= GlobalResources.ManageRolesLabel %></li>
	</ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<% using (Html.BeginForm("Browse", "Users", FormMethod.Get, new { id = "usersearchform" })) { %>
		<label for="username"><%= GlobalResources.UsernameLabel %></label>
		<%= Html.TextBox("username", ViewData["username"]) %>
		<label for="roleid"><%= GlobalResources.RoleLabel %></label>
		<%= Html.DropDownList(GlobalResources.OptionAll, "roleid", ViewData["roles"] as SelectList) %>
		<label for="isactive"><%= GlobalResources.IsActiveLabel %></label>
		<%= Html.DropDownList(GlobalResources.OptionAll, "isactive", ViewData["isactiveoptions"] as SelectList) %>
		<input type="submit" value="<%= GlobalResources.FilterButtonLabel %>" />
	<% } %>
	<br />
	<% if (ViewData.Model.Count > 0) { %>
		<table class="grid" style="width:100%">
			<thead>
				<tr>
					<th><%= GlobalResources.UsernameLabel %></th>
					<th><%= GlobalResources.EmailLabel %></th>
					<th><%= GlobalResources.IsActiveLabel %></th>
					<th><%= GlobalResources.LastLoginLabel %></th>
					<th><%= GlobalResources.LastLoginfromLabel %></th>
					<th>&nbsp;</th>
				</tr>
			</thead>
			<tbody>
				<% foreach (var user in ViewData.Model) { %>
					<tr>
						<td><%= user.UserName %></td>
						<td><%= user.Email %></td>	
						<td><%= user.IsActive %></td>
						<td><%= user.LastLogin %></td>
						<td><%= user.LastIp %></td>
						<td><%= Html.ActionLink(GlobalResources.EditLabel, "Edit", new { id = user.Id }) %></td>
					</tr>
				<% } %>
			</tbody>
		</table>
		<div class="pager">
			<%= Html.Pager(ViewData.Model.PageSize, ViewData.Model.PageNumber, ViewData.Model.TotalItemCount, new { username = ViewData["username"], roleid = ViewData["roleid"], isactive = ViewData["isactive"] } )%>
		</div>
	<% } else { %>
		<%= GlobalResources.NoUsersFound %>
	<% } %>
</asp:Content>
