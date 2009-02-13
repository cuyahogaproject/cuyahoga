<%@ Page Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<ICollection<Template>>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TasksLabel %></h2>
	<%= Html.ActionLink(GlobalResources.RegisterTemplateLabel, "New", null, new { @class = "createlink" })%>
	<a href="#" class="expandlink"><%= GlobalResources.UploadNewTemplateFilesLabel %></a>
	<div id="uploadarea" class="taskcontainer" style="display:none">
		<% using (Html.BeginForm("UploadTemplates", "Templates", FormMethod.Post, new { id = "templatesuploadform", enctype = "multipart/formdata" })) { %>
            <p>
            <%= GlobalResources.UploadNewTemplateFilesHint %>            
            </p>
            <input type="file" id="templatesuploader" name="templatesuploader" style="width:220px" /><br />
            <input type="submit" value="Upload" />
        <% } %>
        <script type="text/javascript">
			$(document).ready(function() {
			
				$('#templatesuploadform').ajaxForm({ 
					dataType:  'json', 
					success:   processJsonMessage // in cuyahoga.common.js
				}); 
			
			});        
		</script> 
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<% if (ViewData.Model.Count > 0) { %>
		<table class="grid" style="width:100%">
			<thead>
				<tr>
					<th><%= GlobalResources.NameLabel%></th>
					<th><%= GlobalResources.BasePathLabel%></th>
					<th><%= GlobalResources.TemplateControlLabel%></th>
					<th><%= GlobalResources.CssLabel%></th>
					<th>&nbsp;</th>
				</tr>
			</thead>
			<tbody>
				<% foreach (var template in ViewData.Model) { %>
					<tr>
						<td><%= template.Name%></td>
						<td><%= template.BasePath%></td>	
						<td><%= template.TemplateControl%></td>
						<td><%= template.Css%></td>
						<td style="white-space:nowrap">
							<%= Html.ActionLink(GlobalResources.ViewLabel, "View", new { id = template.Id })%>
							<%= Html.ActionLink(GlobalResources.EditLabel, "Edit", new { id = template.Id })%>
							<% using (Html.BeginForm("Delete", "Templates", new { id = template.Id }, FormMethod.Post)) { %>
								<a href="#" class="deletelink"><%= GlobalResources.DeleteButtonLabel%></a>
							<% } %>
						</td>
					</tr>
				<% } %>
			</tbody>
		</table>
	<% } else { %>
		<%= GlobalResources.NoTemplatesFound%>
	<% } %>
</asp:Content>
