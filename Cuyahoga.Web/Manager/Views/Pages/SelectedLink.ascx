<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Node>" %>
<h2><%= GlobalResources.LinkPropertiesLabel %></h2>
<div class="taskcontainer">
	<% using (Html.BeginForm("SaveLinkProperties", "Pages", new { id = ViewData.Model.Id }, FormMethod.Post)) { %>
		<p>
			<label for="Title"><%= GlobalResources.LinkTitleLabel %></label>
			<%= Html.TextBox("Title", ViewData.Model.Title, new { style = "width:90%;" })%>
		</p>
		<p>
			<label for="LinkUrl"><%= GlobalResources.LinkUrlLabel %></label><br />
			<%= Html.TextBox("LinkUrl", ViewData.Model.LinkUrl, new { style = "width:90%;" })%>
		</p>
		<p>
			<label for="LinkTarget"><%= GlobalResources.LinkTargetLabel %></label>
			<%= Html.DropDownList("LinkTarget", ViewData["LinkTargets"] as SelectList, new { style = "width:90%;" })%>
		</p>
		<input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" />
	<% } %>
</div>