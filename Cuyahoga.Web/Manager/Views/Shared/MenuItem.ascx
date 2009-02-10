<%@ Import Namespace="Cuyahoga.Web.Manager.Model.ViewModels"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MenuItemData>" %>
<% if (ViewData.Model.IsSelected) { %>
<li class="selected">
<% } else { %>
<li>
<% } %>
	<a href="<%= ViewData.Model.Url %>">
	<% if (! String.IsNullOrEmpty(ViewData.Model.IconUrl)) { %>
		<img src="<%= ViewData.Model.IconUrl %>" alt="" />
	<% } %>
	<%= ViewData.Model.Text %></a>
</li>