<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectedPage.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.SelectedPage" %>
<h2>Page properties</h2>
<div class="taskcontainer">
	<% using (Html.BeginForm("SavePageProperties", "Pages", new { id = ViewData.Model.Id }, FormMethod.Post, new { id = "pageform" })) { %>
		<p>
			<label for="Title">Page title</label>
			<%= Html.TextBox("Title", ViewData.Model.Title, new { style = "width:220px;"}) %>
		</p>
		<p>
			<label for="ShortDescription">Page url</label><br />
			/<%= Html.TextBox("ShortDescription", ViewData.Model.ShortDescription, new { style = "width:215px;" })%>
		</p>
		<p>
			<label for="Culture">Page culture</label>
			<%= Html.DropDownList("Culture", ViewData["Cultures"] as SelectList, new { style = "width:225px;" })%>
		</p>
		<p>
			<%= Html.CheckBox("ShowInNavigation", ViewData.Model.ShowInNavigation) %>
			<label for="ShowInNavigation">Show in navigation</label>
		</p>
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
	<% } %>
</div>