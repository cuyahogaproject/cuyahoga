<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Node>" %>
<%@ Import Namespace="Cuyahoga.Core.Util"%>
			<% using (Html.PageListItem(Model, ViewData["ActiveNode"] as Node)) { %>
				<% using (Html.PageRowDiv(Model, ViewData["ActiveNode"] as Node)) { %>					
					<div class="<%= Model.IsExternalLink ? "link" : "page" %>">
						<%= Html.PageExpander(Model, ViewData["ActiveNode"] as Node)%>
						<%= Html.PageImage(Model)%>
					</div>
					<div class="pagecommands">
						<% if (! Model.IsExternalLink) { %>
							<%= Html.ActionLink(GlobalResources.EditLayoutLabel, "Design", "Pages", new { id = Model.Id }, new { @class = "designlink" })%>
						<% } %>
						<% if (! Model.IsExternalLink) { %>
							<%= Html.ActionLink(GlobalResources.EditContentLabel, "Content", "Pages", new { id = Model.Id }, new { @class = "contentlink" })%>
						<% } %>
					</div>
					<div class="pageinfo">
						<div>
							<span class="pagetitle">
								<%= Model.Title %>
							</span>
							<span class="pageurl">
								<%= Model.DisplayUrl %>
							</span>
						</div>
						<div class="pagesub">
							<span><%= GlobalResources.TemplateLabel %>: <%= Model.Template != null ? Model.Template.Name : String.Empty %></span>
							<span><%= GlobalResources.CultureLabel %>: <%= Model.Culture %></span>
							<span><%= GlobalResources.LastModifiedLabel %>: <%= Model.UpdateTimestamp %></span>
						</div>
					</div>
				<% } %>
				<% if (Model.Level < 1 || (Model.IsInPath((Node)ViewData["ActiveNode"]))) { %>
					<% Html.RenderPartial("PageListItems", Model.ChildNodes, ViewData); %>
				<% } %>	
			<% } %>
				