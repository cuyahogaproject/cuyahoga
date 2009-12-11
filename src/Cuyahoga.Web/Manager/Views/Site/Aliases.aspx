<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SiteAlias>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= String.Format(GlobalResources.ManageAliasesPageTitle, ((Site)ViewData["Site"]).Name) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TasksLabel %></h2>
	<%= Html.ActionLink(GlobalResources.NewAliasLabel, "NewAlias", null, new { @class = "createlink" })%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
<%
	var currentSite = (Site) ViewData["Site"]; 
%>
	<h1><%= String.Format(GlobalResources.ManageAliasesPageTitle, currentSite.Name)%> (<%= currentSite.SiteUrl %>)</h1>
	<table class="grid">
		<thead>
			<tr>
				<th><%= GlobalResources.AliasUrlLabel %></th>
				<th><%= GlobalResources.EntryPageLabel %></th>
				<th>&nbsp;</th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var siteAlias in Model) { %>
				<tr>
					<td><%= siteAlias.Url %></td>
					<td><%= siteAlias.EntryNode != null ? siteAlias.EntryNode.Title : String.Empty %></td>
					<td>
						<%= Html.ActionLink(GlobalResources.EditLabel, "EditAlias", new { id = siteAlias.Id }, new { @class = "abtnedit" })%>
						<% using (Html.BeginForm("DeleteAlias", "Site", new { id = siteAlias.Id }, FormMethod.Post)) { %>
							<a href="#" class="abtndelete"><%= GlobalResources.DeleteButtonLabel%></a>
						<% } %>
				</td>
				</tr>
			<% } %>
		</tbody>
	</table>
</asp:Content>
