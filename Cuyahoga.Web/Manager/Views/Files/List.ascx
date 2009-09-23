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
				<th style="width:5%">&nbsp;</th>
				<th><%= GlobalResources.NameLabel %></th>
				<th style="width:10%; text-align:center;"><%= GlobalResources.SizeLabel %></th>
				<th style="width:25%; text-align:center;"><%= GlobalResources.DateModifiedLabel %></th>
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
					<td>&nbsp;</td>
					<td><%= directory.LastWriteTime.ToString() %></td>
				</tr>
			<% } %>
			<% foreach (var file in Model.Files) { %>
				<tr>
					<td><input type="checkbox" name="files" value="<%= Model.Path + file.Name %>" /></td>
					<td>
						<%= Html.FileImage("~/manager/Content/Images/", file.Name) %>
						<%= file.Name %>
					</td>
					<td style="text-align:center;"><%= file.Length %></td>
					<td style="text-align:center;"><%= file.LastWriteTime.ToString() %></td>
				</tr>
			<% } %>
		</tbody>
	</table>

	<% if (Model.CanCopy || Model.CanMove || Model.CanDelete) { %>
		<p id="fileactions" style="display:none">
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
			<select name="pathto" id="pathto"></select>
			<input id="fileactionbutton" type="submit" value="<%= GlobalResources.OkLabel %>" />
		</p>
	<% } // end if %>
<% } // end form %>

<form action="<%= Url.Action("Upload", "Files") %>" method="post" enctype="multipart/form-data">
	<div id="uploadpanel">
		<%= Html.Hidden("uploadpath", Model.Path)%>
	    <label for="filedata"><%=GlobalResources.UploadFilesLabel %> :</label>
	    <input type="file" name="filedata" id="filedata" />
	    <input id="uploadbutton" class="abtnupload" type="submit" value="<%= GlobalResources.UploadButtonLabel %>" />
	</div>
</form>

<% if (Model.CanCreateDirectory) { %>
	<% using (Html.BeginForm("CreateDirectory", "Files")) { %>
		<div id="createdirpanel">
			<%=Html.Hidden("parentpath", Model.Path)%>
			<label for="name"><%= GlobalResources.CreateDirectoryLabel %> :</label>
			<%=Html.TextBox("name")%>
			<input type="submit" class="abtnok" value="<%= GlobalResources.OkLabel %>" />
		</div>
	<% } %>
<% } %>