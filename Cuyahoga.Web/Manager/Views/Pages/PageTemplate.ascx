<%@ Import Namespace="Cuyahoga.Web.Manager.Model.ViewModels"%>
<%@ Control Language="C#" Inherits="ViewUserControl<TemplateViewData>" %>
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