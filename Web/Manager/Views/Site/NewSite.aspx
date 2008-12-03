<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="NewSite.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Site.NewSite" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<% using (Html.BeginForm("Create", "Site", FormMethod.Post, new { id = "siteform" })) { %>
		<fieldset>
			<legend><%=GlobalResources.GeneralLabel%></legend>
			<ol>
				<li>
					<label for="Name"><%=GlobalResources.NameLabel%></label>
					<%=Html.TextBox("Name", ViewData.Model.Name, new { style = "width:300px" })%>
				</li>				
				<li>
					<label for="SiteUrl"><%=GlobalResources.SiteUrlLabel%></label>
					<%=Html.TextBox("SiteUrl", ViewData.Model.SiteUrl, new { style = "width:300px" })%>
				</li>
				<li>
					<label for="WebmasterEmail"><%=GlobalResources.WebmasterEmailLabel%></label>
					<%=Html.TextBox("WebmasterEmail", ViewData.Model.WebmasterEmail, new { style = "width:300px" })%>
				</li>
				<li>
					<label for="UseFriendlyUrls"><%=GlobalResources.UseFriendlyUrlsLabel%></label>
					<%= Html.CheckBox("UseFriendlyUrls", ViewData.Model.UseFriendlyUrls)%>
				</li>
			</ol>
		</fieldset>
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
		</fieldset>
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
		<%= GlobalResources.Or %>
		<%= Html.ActionLink(GlobalResources.CancelLabel, "Index", "Site") %>
	<% } %>
</asp:Content>
