<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Permission>>" %>
<%= GlobalResources.ViewRolesLabel %>
<% if (ViewData.ContainsKey("AllRoles")) { %>
	<ul>
	<% foreach (Role role in (IEnumerable)ViewData["AllRoles"]) { %>
		<li>
			<input type="checkbox" name="ViewRoleIds" id="viewrole_<%= role.Id %>" value="<%= role.Id %>" <%= ViewData.Model.Any(p => p.Role == role && p.ViewAllowed) ? "checked" : String.Empty %> />
			<label for="viewrole_<%= role.Id %>"><%= role.Name %></label>
		</li>
	<% } %>
	</ul>
<% } %>
<%= GlobalResources.EditRolesLabel %>
<% if (ViewData.ContainsKey("EditorRoles")) { %>
	<ul>
	<% foreach (Role role in (IEnumerable)ViewData["EditorRoles"]) { %>
		<li>
			<input type="checkbox" name="EditRoleIds" id="editrole_<%= role.Id %>" value="<%= role.Id %>" <%= ViewData.Model.Any(p => p.Role == role && p.EditAllowed) ? "checked" : String.Empty %> />
			<label for="editrole_<%= role.Id %>"><%= role.Name %></label>
		</li>
	<% } %>
	</ul>
<% } %>
