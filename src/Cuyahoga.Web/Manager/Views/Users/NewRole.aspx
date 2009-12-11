<%@ Page Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Role>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Import Namespace="Cuyahoga.Core.Service.Membership"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.NewRolePageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= GlobalResources.NewRolePageTitle %></h1>
	<% using(Html.BeginForm("CreateRole", "Users", FormMethod.Post, new { id = "roleform" })) { %>
		<fieldset>
			<legend><%=GlobalResources.GeneralLabel%></legend>
			<ol>
				<li>
					<label for="role.Name"><%=GlobalResources.NameLabel%></label>
					<%=Html.TextBox("role.Name", Model.Name)%>
				</li>
				<% if (Html.HasRight(User, Rights.GlobalPermissions)) { %>
				<li>
					<label for="role.IsGlobal"><%=GlobalResources.IsGlobalLabel%></label>
					<%= Html.CheckBox("role.IsGlobal", Model.IsGlobal)%>
				</li>
				<% } %>
			</ol>
		</fieldset>
		
		<% Html.RenderPartial("RightsSelector", Model, ViewData); %>
		
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
		<%= GlobalResources.Or %>
		<%= Html.ActionLink(GlobalResources.CancelLabel, "Roles", "Users") %>
		<%= Html.ClientSideValidation(ViewData.Model, "roleform", prop => "'role." + prop + "'") %>
	<% } %>
</asp:Content>
