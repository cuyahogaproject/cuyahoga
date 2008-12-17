<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageListItem.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.PageListItem" %>
			<% using (Html.PageRow(ViewData.Model, ViewData["ActiveNode"] as Node)) { %>
				<td>
					<span style="margin-left:<%= ViewData.Model.Level * 20 %>px">
						<%= Html.PageExpander(ViewData.Model, ViewData["ActiveNode"] as Node)%>
						<%= Html.PageImage(ViewData.Model)%>
						<%= ViewData.Model.Title%>
					</span>
				</td>
				<td><%= ViewData.Model.DisplayUrl%></td>
				<td><%= ViewData.Model.Template != null ? ViewData.Model.Template.Name : String.Empty%></td>
				<td class="center"><%= ViewData.Model.Culture%></td>
				<td class="right"><%= ViewData.Model.UpdateTimestamp%></td>
			<% } %>
			<% if (ViewData.Model.Level < 0 || (ViewData.Model.IsInPath((Node)ViewData["ActiveNode"]))) { %>
				<% Html.RenderPartial("PageListItems", ViewData.Model.ChildNodes, ViewData); %>
			<% } %>		