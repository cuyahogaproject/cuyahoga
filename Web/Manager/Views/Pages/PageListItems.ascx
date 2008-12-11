<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageListItems.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.PageListItems" %>
		<% foreach (var node in this.ViewData.Model) { %>
			<% Html.RenderPartial("PageListItem", node, ViewData); %>	
		<% } %>