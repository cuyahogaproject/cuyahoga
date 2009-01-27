<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharedSectionElements.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Shared.SharedSectionElements" %>
	<fieldset>
		<ol>
			<li>
				<label>Module type</label>
				<%= ViewData.Model.ModuleType.Name %>
			</li>
			<li>
				<label>Placeholder</label>
				<%= ViewData.Model.PlaceholderId %>
			</li>
			<li>
				<label for="section.Title">Title</label>
				<%= Html.TextBox("section.Title", ViewData.Model.Title, new { style = "width:300px" } ) %>
			</li>
			<li>
				<label for="section.ShowTitle">Show section title</label>
				<%= Html.CheckBox("section.ShowTitle", ViewData.Model.ShowTitle) %>
			</li>
			<li>
				<label for="section.CacheDuration">Cache duration</label>
				<%= Html.TextBox("section.CacheDuration", ViewData.Model.CacheDuration, new { style = "width:40px"} ) %>
			</li>
	<% if (ViewData.Model.ModuleType.ModuleSettings.Count > 0) {  %>
		<ol>
			<% foreach (ModuleSetting moduleSetting in ViewData.Model.ModuleType.ModuleSettings) {
				string name = "settings_" + moduleSetting.Name;
				%>
				<li>
					<label for="<%= name %>"><%= moduleSetting.FriendlyName %></label>
					<%= Html.SectionSetting(moduleSetting, name, ViewData.Model.Settings[moduleSetting.Name], true) %>
				</li>
			<% } %>
		
	<% } %>
		</ol>
	</fieldset>