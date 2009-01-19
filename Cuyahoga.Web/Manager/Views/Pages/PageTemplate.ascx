<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageTemplate.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.PageTemplate" %>
<style type="text/css">
<%= ViewData.Model.TemplateCss %>
</style>
<div id="<%= ViewData.Model.CssIdPrefix %>">
	<%= ViewData.Model.TemplateHtml %>
</div>
<div id="plh-unknown" class="contentplaceholder">
	<div class="placeholdertitle">Orphaned sections</div>
	<ul class="sectionlist">
	
	</ul>
</div>