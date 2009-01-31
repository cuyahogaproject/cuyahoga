<%@ Import Namespace="Cuyahoga.Web.Manager.Model.ViewModels"%>
<%@ Control Language="C#" Inherits="ViewUserControl<MenuViewData>" %>
<% if (ViewData.Model.SubMenuItems.Count > 0) { %>
	<div id="submenu">
		<ul>
			<% foreach(var menuItem in ViewData.Model.SubMenuItems) {  %>
				<% Html.RenderPartial("MenuItem", menuItem); %>
			<% } %>
		</ul>
	</div>
<% } %>