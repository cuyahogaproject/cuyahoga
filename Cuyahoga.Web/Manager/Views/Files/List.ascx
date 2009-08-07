<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DirectoryViewData>" %>
<%@ Import Namespace="Cuyahoga.Web.Manager.Model.ViewModels"%>

Path: <%= Model.Path %>

<table class="grid">
	<thead>
		<tr>
			<th>&nbsp;</th>
			<th>Name</th>
			<th>Size</th>
			<th>Date modified</th>
		</tr>
	</thead>
	<tbody>
		<% if (Model.ParentDirectory != null) { %>
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
		<% foreach (var directory in Model.SubDirectories) { %>
			<tr>
				<td><%= Html.CheckBox("d_" + directory) %></td>
				<td>
					<a href="<%= Url.Action("List", new { path = Model.Path + directory.Name + "/" }) %>">
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
				<td><%= Html.CheckBox("f_" + file) %></td>
				<td><%= file.Name %></td>
				<td class="right"><%= file.Length %></td>
				<td class="right"><%= file.LastWriteTime.ToString() %></td>
			</tr>
		<% } %>
	</tbody>
</table>