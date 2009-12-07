<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<SiteAlias>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= String.Format(GlobalResources.NewSiteAliasPageTitle, ((Site)ViewData["Site"]).Name)%></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= String.Format(GlobalResources.NewSiteAliasPageTitle, ((Site)ViewData["Site"]).Name)%></h1>
	<% using (Html.BeginForm("CreateAlias", "Site", FormMethod.Post, new { @id = "sitealiasform" })) { %>
		<fieldset>
			<ol>
				<li>
					<label for="Url"><%= GlobalResources.AliasUrlHttpLabel %></label>
					<%= Html.TextBox("Url", Model.Url, new { style = "width:300px" })%>
				</li>
				<li>
					<label for="EntryNodeId"><%= GlobalResources.EntryPageLabel %></label>
					<%= Html.DropDownList("EntryNodeId", ViewData["EntryNodes"] as SelectList, string.Format("--- {0} ---", GlobalResources.SameAsSiteLabel)) %>
				</li>
			</ol>
		</fieldset>
		<div id="buttonpanel">
		    <input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" /> <strong><%= GlobalResources.Or %></strong> <%= Html.ActionLink(GlobalResources.CancelLabel, "Aliases", "Site", null, new { @class = "abtncancel" })%>
		</div>
	<% } %>
</asp:Content>
