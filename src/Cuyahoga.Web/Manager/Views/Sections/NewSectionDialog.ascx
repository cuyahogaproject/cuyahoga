<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Section>" %>
<% using (Html.BeginForm("AddSectionToPage", "Sections", FormMethod.Post, new { @id = "newsectionform" })) { %>
	<%= Html.Hidden("NodeId", ViewData["NodeId"]) %>
	<%= Html.Hidden("ModuleTypeId", ViewData.Model.ModuleType.ModuleTypeId) %>
	<%= Html.Hidden("section.PlaceHolderId", ViewData.Model.PlaceholderId) %>
	<%= Html.Hidden("isvalid", ViewData.ModelState.IsValid) %>
	<% Html.RenderPartial("SharedSectionElements", ViewData.Model); %>
<% } %>
