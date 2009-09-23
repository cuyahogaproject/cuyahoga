<%@ Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IPagedList<User>>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Import Namespace="Cuyahoga.Web.Mvc.Paging"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.ManageUsersPageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TasksLabel %></h2>
	<%= Html.ActionLink(GlobalResources.CreateUserLabel, "New", "Users", null, new { @class = "createlink" }) %>
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= GlobalResources.ManageUsersPageTitle %></h1>
	<% using (Html.BeginForm("Browse", "Users", FormMethod.Get, new { id = "usersearchform" })) { %>
		<label for="username"><%= GlobalResources.UsernameLabel %></label>
		<%= Html.TextBox("username", ViewData["username"]) %>
		<label for="roleid"><%= GlobalResources.RoleLabel %></label>
		<%= Html.DropDownList("roleid", ViewData["roles"] as SelectList, GlobalResources.OptionAll)%>
		<label for="isactive"><%= GlobalResources.IsActiveLabel %></label>
		<%= Html.DropDownList("isactive", ViewData["isactiveoptions"] as SelectList, GlobalResources.OptionAll) %>
		<% if ((bool)ViewData["globalsearchallowed"]) { %>
			<%= Html.CheckBox("globalsearch", (bool?)ViewData["globalsearch"])%><label for="globalsearch"><%= GlobalResources.GlobalSearchLabel %></label>
		<% } %>
		<input type="submit" class="abtnfilter" value="<%= GlobalResources.FilterButtonLabel %>" />
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
						<td>
							<%= Html.ActionLink(GlobalResources.EditLabel, "Edit", new { id = user.Id }, new { @class = "abtnedit" })%>
							<% using (Html.BeginForm("Delete", "Users", new { id = user.Id }, FormMethod.Post))
          { %>
								<a href="#" class="abtndelete"><%= GlobalResources.DeleteButtonLabel %></a>
							<% } %>
						</td>
					</tr>
				<% } %>
			</tbody>
		</table>
		<div class="pager">
			<%= Html.Pager(ViewData.Model.PageSize, ViewData.Model.PageNumber, ViewData.Model.TotalItemCount, new { username = ViewData["username"], roleid = ViewData["roleid"], isactive = ViewData["isactive"], globalsearch = ViewData["globalsearch"] } )%>
		</div>
	<% } else { %>
		<%= GlobalResources.NoUsersFound %>
	<% } %>
</asp:Content>
