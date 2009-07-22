<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ContentItem>" %>
<%
	var categoryIds = String.Join(",", (from category in Model.Categories select category.Id.ToString()).ToArray()); 
	var categoryNames = String.Join(", ", (from category in Model.Categories select category.Name).ToArray());
%>
<%= Html.ScriptInclude("~/Modules/Shared/Scripts/cuyahoga.categorypicker.js") %>
<%= GlobalResources.CategoriesLabel %>: 
<div class="categorycontainer">
	<%= Html.Hidden("categories", categoryIds) %>
	<span class="displaycategories">
		<span class="categorynames"><%= categoryNames %></span>
		<a href="#"><%= GlobalResources.EditLabel %></a>
	</span>

	<span class="categoryeditbuttons" style="display:none">
		<input type="button" value="<%= GlobalResources.OkLabel %>" />
		<a href="#"><%= GlobalResources.CancelLabel %></a>
	</span>
	
	<div class="categorypicker" style="display:none;">
	</div>

</div>