<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharedSectionElements.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Sections.SharedSectionElements" %>
	<fieldset>
		<legend>Common section properties</legend>
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
		</ol>
	</fieldset>
	<fieldset>
		<legend>Custom section properties</legend>
	</fieldset>