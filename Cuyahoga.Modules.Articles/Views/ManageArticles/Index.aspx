<%@ Page Language="C#" MasterPageFile="~/Modules/Shared/Views/Shared/ModuleAdmin.master" Inherits="System.Web.Mvc.ViewPage<ModuleAdminViewModel<IEnumerable<Article>>>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
	<title><%= ArticleResources.ManageArticlesPageTitle %></title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<h2><%= ArticleResources.ManageArticlesPageTitle%></h2>
</asp:Content>

<asp:content id="Content1" contentplaceholderid="MainContent" runat="server">
	<table class="grid" style="width:100%">
		<thead>
			<tr>
				<th><%= ArticleResources.TitleLabel %></th>
				<th><%= ArticleResources.AuthorLabel %></th>
				<th><%= ArticleResources.CreatedLabel %></th>
				<th><%= ArticleResources.PublishedLabel %></th>
				<th>&nbsp;</th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var article in Model.ModuleData) { %>
				<tr>
					<td><%= article.Title %></td>
					<td><%= article.CreatedBy.FullName %></td>
					<td><%= article.CreatedAt %></td>
					<td><%= article.PublishedAt %></td>
					<td>
						<%= Html.ActionLink(GlobalResources.EditLabel, "Edit", Model.GetNodeAndSectionParams().Merge("id", article.Id)) %>
						<% using (Html.BeginForm(GlobalResources.DeleteButtonLabel, "ManageArticles", Model.GetNodeAndSectionParams().Merge("id", article.Id), FormMethod.Post))
		 { %>
							<a href="#" class="deletelink"><%= GlobalResources.DeleteButtonLabel %></a>
						<% } %>
					</td>
				</tr>
			<% } %>
		</tbody>
	</table>
	<p>
		<input type="button" onclick="document.location.href='<%= Url.Action("New", "ManageArticles", Model.GetNodeAndSectionParams()) %>'" value="Create new article" />
	</p>
</asp:content>
