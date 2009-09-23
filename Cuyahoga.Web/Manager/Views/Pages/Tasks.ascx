<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Node>" %>
<h2><%= GlobalResources.TasksLabel %></h2>
<%
	bool isCreatingRootPage = (ViewData["CurrentTask"] as String ?? String.Empty) == "CreateRootPage";
	bool isCreatingPage = (ViewData["CurrentTask"] as String ?? String.Empty) == "CreatePage";
	bool isCreatingLink = (ViewData["CurrentTask"] as String ?? String.Empty) == "CreateLink";
%>
<a href="#" class="<%= (isCreatingRootPage ? " collapselink" : " expandlink") %>"><%= GlobalResources.NewRootPageLabel %></a>
<div class="taskcontainer"<% if (! isCreatingRootPage) { %> style="display:none"<% } %>>
	<% using (Html.BeginForm("CreateRootPage", "Pages", null, FormMethod.Post)) { %>
		<p>
			<label for="Title"><%= GlobalResources.PageTitleLabel %></label>
			<%= Html.TextBox("NewRootPage.Title", String.Empty, new { style = "width:90%;" })%>
		</p>
		<p>
			<label for="Culture"><%= GlobalResources.CultureLabel %></label>
			<%= Html.DropDownList("NewRootPage.Culture", ViewData["AvailableCultures"] as SelectList, new { style = "width:90%;" })%>
		</p>
		<input type="submit" class="abtncreate" value="<%= GlobalResources.CreateButtonLabel %>" />
	<% } %>
</div>
<% if (ViewData.Model.Id > 0) { // a page is selected %>
	<a href="#" class="<%= (isCreatingPage ? " collapselink" : " expandlink") %>"><%= GlobalResources.NewChildPageLabel %></a>
	<div class="taskcontainer"<% if (! isCreatingPage) { %> style="display:none"<% } %>>
		<% using (Html.BeginForm("CreatePage", "Pages", new { parentnodeid = ViewData.Model.Id }, FormMethod.Post)) { %>
			<p>
				<label for="Title"><%= GlobalResources.PageTitleLabel %></label>
				<%= Html.TextBox("NewPage.Title", String.Empty, new { style = "width:90%;" })%>
			</p>
			<input type="submit" class="abtncreate" value="<%= GlobalResources.CreateButtonLabel %>" />
		<% } %>
	</div>
	
	<a href="#" class="<%= (isCreatingLink ? " collapselink" : " expandlink") %>"><%= GlobalResources.NewChildLinkLabel %></a>
	<div class="taskcontainer"<% if (! isCreatingLink) { %> style="display:none"<% } %>>
		<% using (Html.BeginForm("CreateLink", "Pages", new { parentnodeid = ViewData.Model.Id }, FormMethod.Post)) { %>
			<p>
				<label for="Title"><%= GlobalResources.LinkTitleLabel %></label>
				<%= Html.TextBox("NewLink.Title", String.Empty, new { style = "width:90%;" })%>
			</p>
			<p>
				<label for="LinkUrl"><%= GlobalResources.LinkUrlLabel %></label><br />
				<%= Html.TextBox("NewLink.LinkUrl", String.Empty, new { style = "width:90%;" })%>
			</p>
			<p>
				<label for="LinkTarget"><%= GlobalResources.LinkTargetLabel %></label>
				<%= Html.DropDownList("NewLink.LinkTarget", ViewData["NewLinkTargets"] as SelectList, new { style = "width:90%;" })%>
			</p>
			<input type="submit" class="abtncreate" value="<%= GlobalResources.CreateButtonLabel %>" />
		<% } %>
	</div>
	
	<a href="#" class="expandlink" id="togglePermissionsLink"><%= GlobalResources.ManagePermissionsLabel %></a>
	<div class="taskcontainer" style="display:none">
		<% using (Html.BeginForm("SetPagePermissions", "Pages", new { id = ViewData.Model.Id }, FormMethod.Post)) { %>
			<% Html.RenderPartial("ViewAndEditRolesSelector", ViewData.Model.NodePermissions.OfType<Permission>(), ViewData); %>
			<%= GlobalResources.PropagatePermissionsLabel %>
			<ul>
				<li>
					<%= Html.CheckBox("PropagateToChildPages") %>
					<label for="PropagateToChildPages"><%= GlobalResources.PropagateToChildPagesLabel %></label>
				</li>
				<li>
					<%= Html.CheckBox("PropagateToChildSections", true) %>
					<label for="PropagateToChildSections"><%= GlobalResources.PropagateToChildSectionsLabel %></label>
				</li>
			</ul>
			<input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" />
		<% } %>
	</div>

	<% if (! ViewData.Model.IsExternalLink) { %>
		<%= Html.ActionLink(GlobalResources.ManageContentCurrentPageLabel, "Content", "Pages", new { id = ViewData.Model.Id }, new { @class = "contentlink" }) %>
	<% } %>
		
	<% if (! ViewData.Model.IsExternalLink) { %>
		<%= Html.ActionLink(GlobalResources.DesignCurrentPageLabel, "Design", "Pages", new { id = ViewData.Model.Id }, new { @class = "designlink" }) %>
	<% } %>
		
	<% using (Html.BeginForm("Delete", "Pages", new { id = ViewData.Model.Id }, FormMethod.Post)) { %>
		<% if (ViewData.Model.IsExternalLink) { %>
			<a href="#" class="deletelink"><%= GlobalResources.DeleteLinkLabel%></a>
		<% } else { %>
			<a href="#" class="deletelink"><%= GlobalResources.DeletePageLabel%></a>
		<% } %>
	<% } %>
	
<% } %>