<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleSelector.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Users.RoleSelector" %>
<fieldset>
	<li>
	<fieldset>  
		<legend><%= GlobalResources.RolesLabel %></legend>  
		<ol>  
			<% foreach (Role role in (IEnumerable<Role>)ViewData["Roles"]) { %>
			<li>  
				<input type="checkbox" name="RoleIds" id="Role_<%= role.Id %>" value="<%= role.Id %>" <%= ViewData.Model.IsInRole(role) ? "checked" : String.Empty %> />
				<label for="Role_<%= role.Id %>"><%= role.Name %></label>  
			</li>
			<% } %>
		</ol>
	</fieldset>
</li>
</fieldset>