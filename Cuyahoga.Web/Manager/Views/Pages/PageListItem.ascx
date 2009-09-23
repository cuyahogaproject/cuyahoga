<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Node>" %>
			<% using (Html.PageListItem(ViewData.Model, ViewData["ActiveNode"] as Node)) { %>
				<% using (Html.PageRowDiv(ViewData.Model, ViewData["ActiveNode"] as Node)) { %>
					<div class="fr" style="width:20%;"><%= ViewData.Model.UpdateTimestamp%></div>
					<div class="fr" style="width:10%;"><%= ViewData.Model.Culture%></div>
					<div class="fr" style="width:18%;"><%= ViewData.Model.Template != null ? ViewData.Model.Template.Name : String.Empty%></div>
					<div class="fr" style="width:23%;"><%= ViewData.Model.DisplayUrl%>></div>
					<div style="white-space:nowrap">
						<span class="<%= ViewData.Model.IsExternalLink ? "link" : "page" %>">
							<%= Html.PageExpander(ViewData.Model, ViewData["ActiveNode"] as Node)%>
							<%= Html.PageImage(ViewData.Model)%>
							<%= ViewData.Model.Title%>
						</span>
					</div>
				<% } %>
				<% if (ViewData.Model.Level < 1 || (ViewData.Model.IsInPath((Node)ViewData["ActiveNode"]))) { %>
					<% Html.RenderPartial("PageListItems", ViewData.Model.ChildNodes, ViewData); %>
				<% } %>	
			<% } %>
				