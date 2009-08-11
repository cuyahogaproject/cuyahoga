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
				<th>Name</th>
				<th style="width:100px">Size</th>
				<th style="width:130px">Date modified</th>
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
			With the selected items do:
			<select name="fileaction" id="fileaction">
				<% if (Model.CanCopy) { %>
					<option value="<%= Url.Action("Copy", "Files") %>">copy the selected items to</option>
				<% } %>
				<% if (Model.CanMove) { %>
					<option value="<%= Url.Action("Move", "Files") %>">move the selected items to</option>
				<% } %>
				<% if (Model.CanDelete) { %>
					<option value="<%= Url.Action("Delete", "Files") %>">delete the selected items</option>
				<% } %>
			</select>
			<select name="pathto" id="pathto">
				
			</select>
			<input id="fileactionbutton" type="submit" value="OK" />
		</p>
	<% } // end if %>
<% } // end form %>

<% if (Model.CanCreateDirectory) { %>
	<p>
	<% using (Html.BeginForm("CreateDirectory", "Files")) { %>
		<%=Html.Hidden("parentpath", Model.Path)%>
		Create a new directory:
		<%=Html.TextBox("name")%>
		<input type="submit" value="Create" />
	<% } %>
	</p>
<% } %>