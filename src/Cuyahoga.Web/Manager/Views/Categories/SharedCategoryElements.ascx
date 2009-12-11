<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Category>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<li>
	<label for="Name"><%=GlobalResources.NameLabel%></label>
	<%=Html.TextBox("Name", Model.Name, new { style = "width:300px" })%>
</li>
<li>
	<label for="Description"><%=GlobalResources.DescriptionLabel%></label>
	<%=Html.TextBox("Description", Model.Description, new { style = "width:300px" })%>
</li>
<li>
	<label for="ParentCategoryId"><%= GlobalResources.ParentCategoryLabel %></label>
	<select name="ParentCategoryId" style="width:300px">
		<option value="">---<%= GlobalResources.NoneLabel %>---</option>
		<% foreach (var category in (IEnumerable<Category>) ViewData["ParentCategories"]) {
			bool isSelected = Model.ParentCategory != null && Model.ParentCategory.Id == category.Id; 
		%>
			<option value="<%= category.Id %>"<%= isSelected ? " selected=\"selected\"" : String.Empty %> 
				style="padding-left:<%= category.Level * 10 %>px">
			<%= category.Name %></option>
		<% } %>
	</select>
</li>
				