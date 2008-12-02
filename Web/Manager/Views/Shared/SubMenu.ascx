<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubMenu.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Shared.SubMenu" %>
<% if (ViewData.Model.SubMenuItems.Count > 0) { %>
	<div id="submenu">
		<ul>
			<% foreach(var menuItem in ViewData.Model.SubMenuItems) {  %>
				<% Html.RenderPartial("MenuItem", menuItem); %>
			<% } %>
		</ul>
	</div>
<% } %>