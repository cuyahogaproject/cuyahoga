<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SectionViewData>" %>
<%@ Import Namespace="Cuyahoga.Core.Communication"%>
<h2><%= GlobalResources.SelectedSectionLabel %></h2>
<a href="#" class="collapselink"><%= GlobalResources.SectionPropertiesLabel %></a>
<div class="taskcontainer">
	<% using (Html.BeginForm("UpdateSection", "Sections", new { id = Model.Section.Id }, FormMethod.Post, new { id = "sectionform" })) { %>
		<% Html.RenderPartial("SharedSectionElementsNarrow", Model.Section, ViewData); %>
		<input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" />
	<% } %>
</div>
<a href="#" class="expandlink"><%= GlobalResources.SectionPermissionsLabel %></a>
<div class="taskcontainer" style="display:none">
	<% using (Html.BeginForm("SetSectionPermissions", "Sections", new { id = Model.Section.Id }, FormMethod.Post)) { %>
		<% Html.RenderPartial("ViewAndEditRolesSelector", Model.Section.SectionPermissions.OfType<Permission>(), ViewData); %>
		<input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" />
	<% } %>
</div>
<% if (Model.OutboundActions.Count > 0) { %>
	<% if (Model.ExpandConnections) { %>
	<a href="#" class="collapselink"><%= GlobalResources.SectionConnectionsLabel%></a>
	<div class="taskcontainer">	
	<% } else { %>
	<a href="#" class="expandlink"><%= GlobalResources.SectionConnectionsLabel%></a>
	<div class="taskcontainer" style="display:none">
	<% } %>
		<% Html.RenderPartial("SectionConnections", Model); %>
	</div>
<% } %>