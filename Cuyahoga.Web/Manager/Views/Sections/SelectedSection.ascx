<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Section>" %>
<h2><%= GlobalResources.SelectedSectionLabel %></h2>
<a href="#" class="collapselink"><%= GlobalResources.SectionPropertiesLabel %></a>
<div class="taskcontainer">
	<% using (Html.BeginForm("UpdateSection", "Sections", new { id = ViewData.Model.Id }, FormMethod.Post, new { id = "sectionform" })) { %>
		<% Html.RenderPartial("SharedSectionElementsNarrow", ViewData.Model, ViewData); %>
		<input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" />
	<% } %>
</div>
<a href="#" class="expandlink"><%= GlobalResources.SectionPermissionsLabel %></a>
<div class="taskcontainer" style="display:none">
	<% using (Html.BeginForm("SetSectionPermissions", "Sections", new { id = ViewData.Model.Id }, FormMethod.Post)) { %>
		<% Html.RenderPartial("ViewAndEditRolesSelector", ViewData.Model.SectionPermissions.OfType<Permission>(), ViewData); %>
		<input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" />
	<% } %>
</div>
<a href="#" class="expandlink"><%= GlobalResources.SectionConnectionsLabel %></a>
<div class="taskcontainer" style="display:none">
</div>
