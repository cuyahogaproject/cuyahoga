<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Views/Shared/ModuleAdmin.Master" Inherits="System.Web.Mvc.ViewPage<ModuleAdminViewModel<StaticHtmlContent>>" %>
<%@ Import Namespace="Cuyahoga.Web.Mvc.ViewModels"%>
<%@ Import Namespace="Cuyahoga.Modules.StaticHtml"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title><%= GlobalResources.EditContentPageTitle %></title>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<h2><%= GlobalResources.EditContentPageTitle %></h2>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% using (Html.BeginForm("SaveContent", "ManageContent", Model.GetNodeAndSectionParams(), FormMethod.Post)) { %>
		<%= Html.TextArea("content", Model.ModuleData.Content, new { style = "width:100%;height:300px" }) %>
		<input type="submit" value="Save" />
	<% } %>
</asp:Content>
