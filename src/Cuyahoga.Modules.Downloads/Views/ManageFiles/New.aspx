<%@ Page Language="C#" MasterPageFile="~/Modules/Shared/Views/Shared/ModuleAdmin.master" Inherits="System.Web.Mvc.ViewPage<ModuleAdminViewModel<FileResource>>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
	<title><%= GlobalResources.NewFilePageTitle %></title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<h2><%= GlobalResources.NewFilePageTitle%></h2>
</asp:Content>

<asp:content id="Content1" contentplaceholderid="MainContent" runat="server">
	<% using (Html.BeginForm("Create", "ManageFiles", Model.GetNodeAndSectionParams(), FormMethod.Post, new Dictionary<string, object> { { "id", "fileform" }, { "enctype", "multipart/form-data" } })) { %>
		<% Html.RenderPartial("SharedFileFormElements", Model); %>
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
		<%= Html.ActionLink(GlobalResources.CancelLabel, "Index", Model.GetNodeAndSectionParams()) %>
		<% Html.RenderPartial("Categories", Model.ModuleData); %>
	<% } %>
</asp:content>

