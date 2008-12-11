<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageListItem.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.PageListItem" %>
				<tr id="page-<%= ViewData.Model.Id %>" class="parent-<%= ViewData.Model.ParentNode != null ? ViewData.Model.ParentNode.Id.ToString() : String.Empty %>">
					<td>
						<span style="margin-left:<%= ViewData.Model.Level * 20 %>px">
							<%= Html.PageExpander(ViewData.Model, ViewData["ActiveNode"] as Node) %>
							<%= Html.PageImage(ViewData.Model) %>
							<%= ViewData.Model.Title %>
						</span>
					</td>
					<td>/<%= ViewData.Model.ShortDescription %></td>
					<td><%= ViewData.Model.Template != null ? ViewData.Model.Template.Name : String.Empty %></td>
					<td class="center"><%= ViewData.Model.Culture %></td>
					<td class="right"><%= ViewData.Model.UpdateTimestamp %></td>
					<td></td>
				</tr>
				<% if (ViewData.Model.Level < 0 || (ViewData.Model.IsInPath((Node)ViewData["ActiveNode"]))) { %>
					<% Html.RenderPartial("PageListItems", ViewData.Model.ChildNodes, ViewData); %>
				<% } %>		