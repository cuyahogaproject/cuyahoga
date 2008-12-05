<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Templates.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TasksLabel %></h2>
	<%= Html.ActionLink(GlobalResources.RegisterTemplateLabel, "New", null, new { @class = "createlink" }) %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<% if (ViewData.Model.Count > 0) { %>
		<table class="grid" style="width:100%">
			<thead>
				<tr>
					<th><%= GlobalResources.NameLabel %></th>
					<th><%= GlobalResources.BasePathLabel %></th>
					<th><%= GlobalResources.TemplateControlLabel %></th>
					<th><%= GlobalResources.CssLabel %></th>
					<th>&nbsp;</th>
				</tr>
			</thead>
			<tbody>
				<% foreach (var template in ViewData.Model) { %>
					<tr>
						<td><%= template.Name%></td>
						<td><%= template.BasePath %></td>	
						<td><%= template.TemplateControl %></td>
						<td><%= template.Css %></td>
						<td style="white-space:nowrap">
							<%= Html.ActionLink(GlobalResources.EditLabel, "Edit", new { id = template.Id }) %>
							<% using (Html.BeginForm("Delete", "Templates", new { id = template.Id }, FormMethod.Post)) { %>
								<a href="#" class="deletelink"><%= GlobalResources.DeleteButtonLabel %></a>
							<% } %>
						</td>
					</tr>
				<% } %>
			</tbody>
		</table>
	<% } else { %>
		<%= GlobalResources.NoTemplatesFound %>
	<% } %>
</asp:Content>
