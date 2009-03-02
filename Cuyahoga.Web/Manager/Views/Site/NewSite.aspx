<%@ Page Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Site>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.NewSitePageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= GlobalResources.NewSitePageTitle %></h1>
	<% using (Html.BeginForm("Create", "Site", FormMethod.Post, new { id = "siteform" })) { %>
		
		<% Html.RenderPartial("SharedSiteFormElements", ViewData.Model, ViewData); %>
		
		<fieldset>
			<legend><%=GlobalResources.DefaultsLabel%></legend>
			<ol>
				<li>
					<label for="DefaultCulture"><%=GlobalResources.DefaultCultureLabel%></label>
					<%=Html.DropDownList("DefaultCulture", ViewData["Cultures"] as SelectList)%>
				</li>
				<li>
					<label for="DefaultRoleId"><%=GlobalResources.DefaultRoleLabel%></label>
					<%=Html.DropDownList("DefaultRoleId", ViewData["Roles"] as SelectList)%>
				</li>
			</ol>
		</fieldset>		
		<fieldset>
			<legend><%= GlobalResources.TemplatesLabel %></legend>
			<ol>
				<li>
					<fieldset>  
						<legend><%= GlobalResources.CopyTemplatesLabel %></legend>  
						<ol>  
							<% foreach (Template template in (IEnumerable<Template>)ViewData["Templates"]) { %>
							<li>  
								<input type="checkbox" name="TemplateIds" id="Template_<%= template.Id %>" value="<%= template.Id %>" />
								<label for="Template_<%= template.Id %>"><%= template.Name %></label>  
							</li>
							<% } %>
						</ol>
					</fieldset>
				</li>
			</ol>
		</fieldset>
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
		<%= GlobalResources.Or %>
		<%= Html.ActionLink(GlobalResources.CancelLabel, "Index", "Site") %>
	<% } %>
</asp:Content>
