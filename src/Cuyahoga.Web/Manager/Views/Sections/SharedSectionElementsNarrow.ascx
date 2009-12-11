<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Section>" %>	
	<p>
		<label>Module type:</label> <strong><%= ViewData.Model.ModuleType.Name %></strong>
	</p>
	<p>
		<label>Placeholder:</label> <strong><%= ViewData.Model.PlaceholderId %></strong>
	</p>
	<p>
		<label for="section.Title">Title</label><br />
		<%= Html.TextBox("section.Title", ViewData.Model.Title) %>
	</p>
	<p>
		<%= Html.CheckBox("section.ShowTitle", ViewData.Model.ShowTitle) %>
		<label for="section.ShowTitle">Show section title</label>
	</p>
	<p>
		<label for="section.CacheDuration">Cache duration</label><br />
		<%= Html.TextBox("section.CacheDuration", ViewData.Model.CacheDuration)%>
	</p>
	<% if (ViewData.Model.ModuleType.ModuleSettings.Count > 0) {  %>
		<% foreach (ModuleSetting moduleSetting in ViewData.Model.ModuleType.ModuleSettings) {
			string name = "section.Settings_" + moduleSetting.Name;
			if (moduleSetting.SettingDataType == "System.Boolean") {
			%>
				<p>
					<%= Html.SectionSetting(moduleSetting, name, ViewData.Model.Settings[moduleSetting.Name]) %>
					<label for="<%= name %>"><%= moduleSetting.FriendlyName%></label>
				</p>
			<% } else { %>
				<p>
					<label for="<%= name %>"><%= moduleSetting.FriendlyName%></label><br />
					<%= Html.SectionSetting(moduleSetting, name, ViewData.Model.Settings[moduleSetting.Name]) %>
				</p>
			<% } %>
		<% } %>	
	<% } %>