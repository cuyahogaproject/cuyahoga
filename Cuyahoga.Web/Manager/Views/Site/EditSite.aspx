<%@ Page Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Site>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Import Namespace="Cuyahoga.Web.Manager.Helpers"%>
<%@ Import Namespace="Cuyahoga.Core.Service.Membership"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.ManageSitePageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TasksLabel %></h2>
	<% if (Html.HasRight(User, Rights.CreateSite)) { %>
		<%= Html.ActionLink(GlobalResources.CreateSiteLabel, "New", null, new { @class = "createlink"} )%>
	<% } %>
	<% if (Html.HasRight(User, Rights.ManageSite)) { %>
		<%= Html.ActionLink(GlobalResources.ManageAliasesLabel, "Aliases", "Site", null, new { @class = "aliaslink"} )%>
		<%= Html.ActionLink(GlobalResources.RebuildFullTextIndexLabel, "Index", "SearchIndex", null, new { @class = "refreshlink"} )%>
	<% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= GlobalResources.ManageSitePageTitle %></h1>
	<script type="text/javascript">
		$(document).ready(function() {
			$('#DefaultTemplateId').change(function() {
				if ($('select#DefaultTemplateId')) {
					$('#DefaultPlaceholder').html('');
					$.getJSON('<%= Url.Action("GetPlaceholdersByTemplateId", "Templates") %>', { templateid:$('#DefaultTemplateId').val() }, function(data) {
						$.each(data, function(i, item) {
							$('#DefaultPlaceholder').append('<option value="' + item.Placeholder + '">' + item.Placeholder + '</option>');
						});
					});
				}
			});
		});
	</script>
	<% using (Html.BeginForm("Update", "Site", new { id = ViewData.Model.Id }, FormMethod.Post, new { id = "siteform" })) { %>
		
		<% Html.RenderPartial("SharedSiteFormElements", ViewData.Model, ViewData); %>
		
		<fieldset>
			<legend><%=GlobalResources.DefaultsLabel%></legend>
			<ol>
				<li>
					<label for="DefaultCulture"><%= GlobalResources.DefaultCultureLabel %></label>
					<%= Html.DropDownList("DefaultCulture", ViewData["Cultures"] as SelectList)%>
				</li>
				<li>
					<label for="DefaultRoleId"><%= GlobalResources.DefaultRoleLabel %></label>
					<%= Html.DropDownList("DefaultRoleId", ViewData["Roles"] as SelectList)%>
				</li>
				<li>
					<label for="DefaultTemplateId"><%= GlobalResources.DefaultTemplateLabel %></label>
					<%= Html.DropDownList("DefaultTemplateId", ViewData["Templates"] as SelectList)%>
				</li>
				<li>
					<label for="DefaultPlaceholder"><%= GlobalResources.DefaultPlaceholderLabel %></label>
					<% if (ViewData.ContainsKey("PlaceHolders")){ %>
						<%= Html.DropDownList("DefaultPlaceholder", ViewData["Placeholders"] as SelectList)%>
					<% }else{ %>
						<%= Html.TextBox("DefaultPlaceholder", ViewData["DefaultPlaceholder"])%>
					<% } %>
				</li>
				<li>
					<label for="MetaDescription"><%= GlobalResources.DefaultMetaDescriptionLabel %></label>
					<%= Html.TextArea("MetaDescription", ViewData.Model.MetaDescription, new { style = "width:400px" })%>
				</li>
				<li>
					<label for="MetaKeywords"><%= GlobalResources.DefaultMetaKeywordsLabel %></label>
					<%= Html.TextArea("MetaKeywords", ViewData.Model.MetaKeywords, new { style = "width:400px" })%>
				</li>
			</ol>
		</fieldset>	
		<div id="buttonpanel">
		    <input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" />
		</div>
	<% } %>
</asp:Content>
