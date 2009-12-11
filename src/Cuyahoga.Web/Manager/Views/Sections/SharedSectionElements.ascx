<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Section>" %>
	<fieldset>
		<ol>
			<li>
				<label>Module type</label>
				<%= Model.ModuleType.Name %>
			</li>
			<li>
				<label>Placeholder</label>
				<%= Model.PlaceholderId %>
			</li>
			<li>
				<label for="section.Title">Title</label>
				<%= Html.TextBox("section.Title", Model.Title, new { style = "width:300px" } ) %>
			</li>
			<li>
				<label for="section.ShowTitle">Show section title</label>
				<%= Html.CheckBox("section.ShowTitle", Model.ShowTitle) %>
			</li>
			<li>
				<label for="section.CacheDuration">Cache duration</label>
				<%= Html.TextBox("section.CacheDuration", Model.CacheDuration, new { style = "width:40px"} ) %>
			</li>
	<% if (Model.ModuleType.ModuleSettings.Count > 0) {  %>
		<ol>
			<% foreach (ModuleSetting moduleSetting in Model.ModuleType.ModuleSettings) {
				string name = "section.Settings_" + moduleSetting.Name;
				%>
				<li>
					<label for="<%= name %>"><%= moduleSetting.FriendlyName %></label>
					<%= Html.SectionSetting(moduleSetting, name, Model.Settings[moduleSetting.Name], true) %>
				</li>
			<% } %>
		
	<% } %>
		</ol>
	</fieldset>