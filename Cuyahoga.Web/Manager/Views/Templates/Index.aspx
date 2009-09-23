<%@ Page Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<ICollection<Template>>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.ManageTemplatesPageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TasksLabel %></h2>
	<%= Html.ActionLink(GlobalResources.RegisterTemplateLabel, "New", null, new { @class = "createlink" })%>
	<a href="#" class="expandlink"><%= GlobalResources.UploadNewTemplateFilesLabel %></a>
	<div id="uploadarea" class="taskcontainer" style="display:none">
		<% using (Html.BeginForm("UploadTemplates", "Templates", FormMethod.Post, new { id = "templatesuploadform", enctype = "multipart/formdata" })) { %>
            <p>
            <%= GlobalResources.UploadNewTemplateFilesHint %>            
            </p>
            <input type="file" id="templatesuploader" name="templatesuploader" style="width:90%" /><br />
            <input type="submit" class="abtnupload" value="Upload" />
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
	<h1><%= GlobalResources.ManageTemplatesPageTitle %></h1>
	<% if (ViewData.Model.Count > 0) { %>
		<table class="grid" style="width:100%">
			<thead>
				<tr>
					<th><h3><%= GlobalResources.NameLabel%></h3></th>
					<th><h3><%= GlobalResources.BasePathLabel%></h3></th>
					<th><h3><%= GlobalResources.TemplateControlLabel%></h3></th>
					<th><h3><%= GlobalResources.CssLabel%></h3></th>
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
							<%= Html.ActionLink(GlobalResources.ViewLabel, "View", new { id = template.Id }, new { @class = "abtnview" })%>
							<%= Html.ActionLink(GlobalResources.EditLabel, "Edit", new { id = template.Id }, new { @class = "abtnedit" })%>
							<% using (Html.BeginForm("Delete", "Templates", new { id = template.Id }, FormMethod.Post)) { %>
								<a href="#" class="abtndelete"><%= GlobalResources.DeleteButtonLabel%></a>
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
