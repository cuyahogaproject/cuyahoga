<%@ Import Namespace="Cuyahoga.Web.Manager.Model.ViewModels"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MenuItemData>" %>
<li<%= Model.IsSelected ? " class=\"selected\"" : String.Empty %>>
	<a href="<%= Model.Url %>">
	<% if (! String.IsNullOrEmpty(Model.IconUrl)) { %>
		<img src="<%= Model.IconUrl %>" alt="" />
	<% } %>
	<%= ViewData.Model.Text %></a>
	<% if (Model.ChildMenuItems.Count > 0) { %>
		<div class="expander"><img src="<%= Url.Content("~/manager/Content/Images/bullet_arrow_down.png") %>" alt="expand" /></div>
		<ul>
			<% foreach (var subMenuItem in Model.ChildMenuItems) { %>
				<li<%= subMenuItem.IsSelected ? " class=\"selected\"" : String.Empty %>>
					<a href="<%= subMenuItem.Url %>">
					<% if (! String.IsNullOrEmpty(subMenuItem.IconUrl)) { %>
						<img src="<%= subMenuItem.IconUrl %>" alt="" />
					<% } %>	
					<%= subMenuItem.Text %></a>
				</li>
			<% } %>
		</ul>
	<% } %>
</li>