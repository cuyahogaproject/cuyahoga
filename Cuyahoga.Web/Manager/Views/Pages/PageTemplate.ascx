<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageTemplate.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.PageTemplate" %>
<style type="text/css">
<%= ViewData.Model.TemplateCss %>
</style>
<div id="<%= ViewData.Model.CssIdPrefix %>">
	<%= ViewData.Model.TemplateHtml %>
</div>