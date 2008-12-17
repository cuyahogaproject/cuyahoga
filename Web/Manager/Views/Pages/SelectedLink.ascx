<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectedLink.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.SelectedLink" %>
<h2>Link properties</h2>
<div class="taskcontainer">
	<% using (Html.BeginForm("SaveLinkProperties", "Pages", new { id = ViewData.Model.Id }, FormMethod.Post)) { %>
		<p>
			<label for="Title">Link title</label>
			<%= Html.TextBox("Title", ViewData.Model.Title, new { style = "width:220px;"}) %>
		</p>
		<p>
			<label for="LinkUrl">Link url</label><br />
			<%= Html.TextBox("LinkUrl", ViewData.Model.LinkUrl, new { style = "width:220px;" })%>
		</p>
		<p>
			<label for="LinkTarget">Link target</label>
			<%= Html.DropDownList("LinkTarget", ViewData["LinkTargets"] as SelectList, new { style = "width:225px;" })%>
		</p>
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
	<% } %>
</div>