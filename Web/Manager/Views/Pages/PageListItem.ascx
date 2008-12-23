<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageListItem.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.PageListItem" %>
			<% using (Html.PageElement(ViewData.Model, ViewData["ActiveNode"] as Node)) { %>	
				<div class="pagerow">
					<div class="fr" style="width:120px;text-align:right;"><%= ViewData.Model.UpdateTimestamp%></div>
					<div class="fr" style="width:80px"><%= ViewData.Model.Culture%></div>
					<div class="fr" style="width:120px"><%= ViewData.Model.Template != null ? ViewData.Model.Template.Name : String.Empty%></div>
					<div class="fr" style="width:160px"><%= ViewData.Model.DisplayUrl%></div>
					<div style="white-space:nowrap">
						<span class="<%= ViewData.Model.IsExternalLink ? "link" : "page" %>">
							<%= Html.PageExpander(ViewData.Model, ViewData["ActiveNode"] as Node)%>
							<%= Html.PageImage(ViewData.Model)%>
							<%= ViewData.Model.Title%>
						</span>
					</div>
				</div>
				<% if (ViewData.Model.Level < 1 || (ViewData.Model.IsInPath((Node)ViewData["ActiveNode"]))) { %>
					<% Html.RenderPartial("PageListItems", ViewData.Model.ChildNodes, ViewData); %>
				<% } %>	
			<% } %>
				