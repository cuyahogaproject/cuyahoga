<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DirectoryViewData>" %>
<%@ Import Namespace="Cuyahoga.Web.Manager.Model.ViewModels"%>

<%= GlobalResources.PathLabel %>:
<% foreach (var trailItem in Model.Trail) { 
	   %><a href="<%= Url.Action("List", new { path = trailItem.Key } ) %>"><%= trailItem.Value %></a>/<% 
   } %>

<% using (Html.BeginForm("Copy", "Files", FormMethod.Post, new { @id = "fileactionform" })) { %>
	<table class="grid" style="width:99%">
		<thead>
			<tr>
				<th style="width:30px">&nbsp;</th>
				<th><%= GlobalResources.NameLabel %></th>
				<th style="width:100px"><%= GlobalResources.SizeLabel %></th>
				<th style="width:130px"><%= GlobalResources.DateModifiedLabel %></th>
			</tr>
		</thead>
		<tbody>
			<% if (Model.ParentDirectory != null)
		 { %>
				<tr>
					<td>&nbsp;</td>
					<td><a href="<%= Url.Action("List", new { path = Model.ParentDirectory }) %>">
						<img src="<%= Url.Content("~/manager/Content/Images/folder.png") %>" alt="Up" />
						..</a>
					</td>
					<td>&nbsp</td>
					<td>&nbsp</td>
				</tr>
			<% } %>
			<% foreach (var directory in Model.SubDirectories) {
				var virtualDirectoryPath = Model.Path + directory.Name + "/";
				%>
				<tr>
					<td><input type="checkbox" name="directories" value="<%= virtualDirectoryPath %>" /></td>
					<td>
						<a href="<%= Url.Action("List", new { path = virtualDirectoryPath }) %>">
							<img src="<%= Url.Content("~/manager/Content/Images/folder.png") %>" alt="Directory" />
							<%= directory.Name %>
						</a>
					</td>
					<td class="right">&nbsp;</td>
					<td class="right"><%= directory.LastWriteTime.ToString() %></td>
				</tr>
			<% } %>
			<% foreach (var file in Model.Files) { %>
				<tr>
					<td><input type="checkbox" name="files" value="<%= Model.Path + file.Name %>" /></td>
					<td>
						<%= Html.FileImage("~/manager/Content/Images/", file.Name) %>
						<%= file.Name %>
					</td>
					<td class="right"><%= file.Length %></td>
					<td class="right"><%= file.LastWriteTime.ToString() %></td>
				</tr>
			<% } %>
		</tbody>
	</table>

	<% if (Model.CanCopy || Model.CanMove || Model.CanDelete) { %>
		<p>
			<%= Html.Hidden("path", Model.Path) %>
			<%= GlobalResources.WithSelectedItemsDo %>:
			<select name="fileaction" id="fileaction">
				<% if (Model.CanCopy) { %>
					<option value="<%= Url.Action("Copy", "Files") %>"><%= GlobalResources.CopySelectedItemsTo %></option>
				<% } %>
				<% if (Model.CanMove) { %>
					<option value="<%= Url.Action("Move", "Files") %>"><%= GlobalResources.MoveSelectedItemsTo %></option>
				<% } %>
				<% if (Model.CanDelete) { %>
					<option value="<%= Url.Action("Delete", "Files") %>"><%= GlobalResources.DeleteSelectedItems %></option>
				<% } %>
			</select>
			<select name="pathto" id="pathto">
				
			</select>
			<input id="fileactionbutton" type="submit" value="<%= GlobalResources.OkLabel %>" />
		</p>
	<% } // end if %>
<% } // end form %>

<% if (Model.CanCreateDirectory) { %>
	<p>
	<% using (Html.BeginForm("CreateDirectory", "Files")) { %>
		<%=Html.Hidden("parentpath", Model.Path)%>
		<%= GlobalResources.CreateDirectoryLabel %>:
		<%=Html.TextBox("name")%>
		<input type="submit" value="<%= GlobalResources.OkLabel %>" />
	<% } %>
	</p>
<% } %>