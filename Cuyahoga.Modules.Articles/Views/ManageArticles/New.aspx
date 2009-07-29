<%@ Page Language="C#" MasterPageFile="~/Modules/Shared/Views/Shared/ModuleAdmin.master" Inherits="System.Web.Mvc.ViewPage<ModuleAdminViewModel<Article>>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
	<title><%= ArticleResources.NewArticlePageTitle %></title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<h2><%= ArticleResources.NewArticlePageTitle %></h2>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<% using (Html.BeginForm("Create", "ManageArticles", Model.GetNodeAndSectionParams(), FormMethod.Post, new Dictionary<string, object> {{"id", "articleform"}})) {%>
		<% Html.RenderPartial("SharedArticleFormElements", Model); %>
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
		<%= GlobalResources.Or %>
		<%= Html.ActionLink(GlobalResources.CancelLabel, "Index", Model.GetNodeAndSectionParams()) %>
	<% } %>
</asp:Content>
