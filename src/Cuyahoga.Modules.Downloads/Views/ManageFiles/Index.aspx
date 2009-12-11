<%@ Page Language="C#" MasterPageFile="~/Modules/Shared/Views/Shared/ModuleAdmin.master" Inherits="System.Web.Mvc.ViewPage<ModuleAdminViewModel<IEnumerable<FileResource>>>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
	<title><%= GlobalResources.ManageFilesPageTitle %></title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<h2><%= GlobalResources.ManageFilesPageTitle%></h2>
</asp:Content>

<asp:content id="Content1" contentplaceholderid="MainContent" runat="server">
	<table class="grid" style="width:100%">
		<thead>
			<tr>
				<th><%= GlobalResources.FileLabel %></th>
				<th><%= GlobalResources.SizeLabel %></th>
				<th><%= GlobalResources.PublisherLabel %></th>
				<th><%= GlobalResources.PublishedLabel %></th>
				<th><%= GlobalResources.DownloadsLabel %></th>
				<th>&nbsp;</th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var fileResource in Model.ModuleData) { %>
				<tr>
					<td>
						<% if (fileResource.Title != fileResource.FileName) { %>
							<%= fileResource.Title %><br />
						<% } %>	
						<%= fileResource.FileName %>
					</td>
					<td class="right"><%= fileResource.Length %></td>
					<td><%= fileResource.ModifiedBy.FullName %></td>
					<td class="right"><%= fileResource.PublishedAt %></td>
					<td class="center"><%= fileResource.DownloadCount %></td>
					<td>
						<%= Html.ActionLink(GlobalResources.EditLabel, "Edit", Model.GetNodeAndSectionParams().Merge("id", fileResource.Id)) %>
						<% using (Html.BeginForm("Delete", "ManageFiles", Model.GetNodeAndSectionParams().Merge("id", fileResource.Id), FormMethod.Post)) { %>
							<a href="#" class="deletelink"><%= GlobalResources.DeleteButtonLabel %></a>
						<% } %>
					</td>
				</tr>
			<% } %>
		</tbody>
	</table>
	<p>
		<input type="button" onclick="document.location.href='<%= Url.Action("New", "ManageFiles", Model.GetNodeAndSectionParams()) %>'" value="<%= GlobalResources.AddNewFileButtonLabel %>" />
	</p>
</asp:content>

