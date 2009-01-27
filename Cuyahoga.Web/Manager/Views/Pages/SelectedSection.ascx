<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectedSection.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.SelectedSection" %>
<h2><%= GlobalResources.SelectedSectionLabel %></h2>
<a href="#" class="collapselink"><%= GlobalResources.SectionPropertiesLabel %></a>
<div class="taskcontainer">
	<% using (Html.BeginForm("UpdateSection", "Sections", new { id = ViewData.Model.Id }, FormMethod.Post, new { id = "sectionform" })) { %>
		<%= Html.Hidden("IsDesign", true) %>
		<% Html.RenderPartial("SharedSectionElementsNarrow", ViewData.Model, ViewData); %>
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
	<% } %>
</div>
<a href="#" class="expandlink"><%= GlobalResources.SectionPermissionsLabel %></a>
<div class="taskcontainer" style="display:none">
</div>
<a href="#" class="expandlink"><%= GlobalResources.SectionConnectionsLabel %></a>
<div class="taskcontainer" style="display:none">
</div>
