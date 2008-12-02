<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Shared.MainMenu" %>
<div id="mainmenu">
	<% if (ViewData.Model.StandardMainMenuItems.Count > 0) { %>
	<div id="mainmenu-standard">
		<ul>
		<% foreach(var menuItem in ViewData.Model.StandardMainMenuItems) {  %>
			<% Html.RenderPartial("MenuItem", menuItem); %>
		<% } %>
		</ul>
	</div>
	<% } %>
	<% if (ViewData.Model.OptionalMainMenuItems.Count > 0) { %>
	<div id="mainmenu-options">	
		<ul>
		<% foreach(var menuItem in ViewData.Model.OptionalMainMenuItems) {  %>
			<% Html.RenderPartial("MenuItem", menuItem); %>
		<% } %>
		</ul>
	</div>
	<% } %>
</div>