<%@ Page Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<TemplateViewData>" %>
<%@ Import Namespace="Cuyahoga.Web.Manager.Model.ViewModels"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<style type="text/css">
	<%= ViewData.Model.TemplateCss %>
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<div class="templatecontainer">
		<div id="<%= ViewData.Model.CssIdPrefix %>">
			<%= ViewData.Model.TemplateHtml %>
		</div>
	</div>
</asp:Content>
