<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="ViewTemplate.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Templates.ViewTemplate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<style type="text/css">
	<%= ViewData.Model.TemplateCss %>
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<div id="<%= ViewData.Model.CssIdPrefix %>" class="templatecontainer">
		<%= ViewData.Model.TemplateHtml %>
	</div>
</asp:Content>
