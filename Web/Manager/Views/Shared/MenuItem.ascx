<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuItem.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Shared.MenuItem" %>
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