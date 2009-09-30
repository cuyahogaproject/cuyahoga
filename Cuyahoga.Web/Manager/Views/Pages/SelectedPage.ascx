<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Node>" %>
<h2><%= GlobalResources.PagePropertiesLabel %></h2>
<div class="taskcontainer">
	<% using (Html.BeginForm("SavePageProperties", "Pages", new { id = ViewData.Model.Id }, FormMethod.Post, new { id = "pageform" })) { %>
		<p>
			<label for="Title"><%= GlobalResources.PageTitleLabel %></label>
			<%= Html.TextBox("Title", ViewData.Model.Title, new { style = "width:97%;" })%>
		</p>
		<p>
			<label for="ShortDescription"><%= GlobalResources.PageUrlLabel %></label><br />
			/<%= Html.TextBox("ShortDescription", ViewData.Model.ShortDescription, new { style = "width:95%;" })%>
		</p>
		<p>
			<label for="Culture"><%= GlobalResources.CultureLabel %></label>
			<%= Html.DropDownList("Culture", ViewData["Cultures"] as SelectList, new { style = "width:98%;" })%>
		</p>
		<p>
			<%= Html.CheckBox("ShowInNavigation", ViewData.Model.ShowInNavigation) %>
			<label for="ShowInNavigation"><%= GlobalResources.ShowInNavigationLabel %></label>
		</p>
		<input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" />
	<% } %>
</div>