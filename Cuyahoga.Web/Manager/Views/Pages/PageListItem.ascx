<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Node>" %>
			<% using (Html.PageListItem(Model, ViewData["ActiveNode"] as Node)) { %>
				<% using (Html.PageRowDiv(Model, ViewData["ActiveNode"] as Node)) { %>					
					<div class="<%= Model.IsExternalLink ? "link" : "page" %>">
						<%= Html.PageExpander(Model, ViewData["ActiveNode"] as Node)%>
						<%= Html.PageImage(Model)%>
					</div>
					<div class="pagecommands">
						<% if (! Model.IsExternalLink) { %>
							<%= Html.ActionLink(GlobalResources.EditContentLabel, "Content", "Pages", new { id = Model.Id }, new { @class = "contentlink" })%>
						<% } %>
							
						<% if (! Model.IsExternalLink) { %>
							<%= Html.ActionLink(GlobalResources.EditLayoutLabel, "Design", "Pages", new { id = Model.Id }, new { @class = "designlink" })%>
						<% } %>
					</div>
					<div class="pageinfo">
						<div class="pagetitle">
							<%= Model.Title %>
						</div>
						<div class="pageurl">
							<%= Model.DisplayUrl %>
						</div>
						<div class="pagesub">
							<span><%= GlobalResources.TemplateLabel %>: <%= Model.Template != null ? Model.Template.Name : String.Empty %></span>
							<span><%= GlobalResources.CultureLabel %>: <%= Model.Culture %></span>
							<span><%= GlobalResources.LastModifiedLabel %>: <%= Model.UpdateTimestamp %></span>
						</div>
					</div>
					<!--
						<div class="fr" style="width:20%;"><%= ViewData.Model.UpdateTimestamp%></div>
						<div class="fr" style="width:10%;"><%= ViewData.Model.Culture%></div>
						<div class="fr" style="width:18%;"><%= ViewData.Model.Template != null ? ViewData.Model.Template.Name : String.Empty%></div>
						<div class="fr" style="width:23%;"><%= ViewData.Model.DisplayUrl%></div>
					-->
				<% } %>
				<% if (Model.Level < 1 || (Model.IsInPath((Node)ViewData["ActiveNode"]))) { %>
					<% Html.RenderPartial("PageListItems", Model.ChildNodes, ViewData); %>
				<% } %>	
			<% } %>
				