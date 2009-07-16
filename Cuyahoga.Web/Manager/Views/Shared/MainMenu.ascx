<%@ Import Namespace="Cuyahoga.Web.Manager.Model.ViewModels"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MenuViewData>" %>
<div id="mainmenu">
	<% if (Model.StandardMainMenuItems.Count > 0) { %>
	<div id="mainmenu-standard">
		<ul>
		<% foreach(var menuItem in Model.StandardMainMenuItems) {  %>
			<% Html.RenderPartial("MenuItem", menuItem); %>
		<% } %>
		</ul>
	</div>
	<% } %>
	<% if (Model.OptionalMainMenuItems.Count > 0) { %>
	<div id="mainmenu-options">	
		<ul>
		<% foreach(var menuItem in Model.OptionalMainMenuItems) {  %>
			<% Html.RenderPartial("MenuItem", menuItem); %>
		<% } %>
		</ul>
	</div>
	<% } %>
</div>
<script type="text/javascript">
	$(document).ready(function() {
		$('.expander').click(function() {
			$(this).next('ul').slideToggle(100);
		})
	});
</script>