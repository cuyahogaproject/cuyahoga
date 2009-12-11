<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Node>>" %>
			<ul class="pagegroup">
			<% foreach (var node in this.ViewData.Model) { %>
				<% Html.RenderPartial("PageListItem", node, ViewData); %>	
			<% } %>
			</ul>