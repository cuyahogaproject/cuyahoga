<%@ Page Language="C#" MasterPageFile="~/Modules/Shared/Views/Shared/ModuleAdmin.master" Inherits="System.Web.Mvc.ViewPage<ModuleAdminViewModel<Article>>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
	<title><%= GlobalResources.EditArticlePageTitle %></title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<h2><%= GlobalResources.EditArticlePageTitle %></h2>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<% using (Html.BeginForm("Update", "ManageArticles", Model.GetNodeAndSectionParams().Merge("id",Model.ModuleData.Id), FormMethod.Post, new Dictionary<string, object> {{"id", "articleform"}})) { %>
		<% Html.RenderPartial("SharedArticleFormElements", Model); %>
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
		<%= Html.ActionLink(GlobalResources.CancelLabel, "Index", Model.GetNodeAndSectionParams()) %>
		<% Html.RenderPartial("Categories", Model.ModuleData); %>
		<%= Html.ClientSideValidation(Model.ModuleData, "articleform") %>
	<% } %>
	<% Html.RenderPartial("Comments", Model.ModuleData); %>
</asp:Content>
