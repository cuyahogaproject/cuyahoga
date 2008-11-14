<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Shared.MainMenu" %>
<div id="mainmenu">
	<% if (ViewData.Model.StandardMainMenuItems.Count > 0) { %>
	<div id="mainmenu-standard">
		<ul>
		<% foreach(var menuItem in ViewData.Model.StandardMainMenuItems) {  %>
			<% if (menuItem.IsSelected) { %>
			<li class="selected">
			<% } else { %>
			<li>
			<% } %>
				<a href="<%= menuItem.Url %>"><%= menuItem.Text %></a>
			</li>
		<% } %>
		</ul>
	</div>
	<% } %>
	<% if (ViewData.Model.OptionalMainMenuItems.Count > 0) { %>
	<div id="mainmenu-options">	
		<ul>
		<% foreach(var menuItem in ViewData.Model.OptionalMainMenuItems) {  %>
			<% if (menuItem.IsSelected) { %>
			<li class="selected">
			<% } else { %>
			<li>
			<% } %>
				<a href="<%= menuItem.Url %>"><%= menuItem.Text %></a>
			</li>
		<% } %>
		</ul>
	</div>
	<% } %>
</div>