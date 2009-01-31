<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="sitechooser">
	<form id="sitechooserform" action="<%= Url.Action("SetSite", "Dashboard") %>" method="post">
	<%= GlobalResources.SiteLabel %>:
	<%= Html.DropDownList("SiteId") %>
	<input type="image" src="<%= Url.Content("~/Manager/Content/Images/world_go.png") %>" value="Go" style="vertical-align:middle;" />
	</form>
</div>