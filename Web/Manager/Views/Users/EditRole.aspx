<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="EditRole.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Users.EditRole" %>
<%@ Import Namespace="Cuyahoga.Core.Service.Membership"%>
<%@ Import Namespace="Cuyahoga.Web.Mvc.Helpers"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<% using (Html.BeginForm("UpdateRole", "Users", new { id = ViewData.Model.Id }, FormMethod.Post, new { id = "roleform" })) { %>
		<fieldset>
			<legend><%=GlobalResources.GeneralLabel%></legend>
			<ol>
				<li>
					<label for="Name"><%=GlobalResources.NameLabel%></label>
					<%=Html.TextBox("Name", ViewData.Model.Name)%>
				</li>
				<% if (Html.HasRight(User, Rights.GlobalPermissions)) { %>
				<li>
					<label for="IsGlobal"><%=GlobalResources.IsGlobalLabel%></label>
					<%= Html.CheckBox("IsGlobal", ViewData.Model.IsGlobal)%>
				</li>
				<% } %>
			</ol>
		</fieldset>
		
		<% Html.RenderPartial("RightsSelector", ViewData.Model, ViewData); %>
		
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
		<%= GlobalResources.Or %>
		<%= Html.ActionLink(GlobalResources.CancelLabel, "Roles", "Users") %>
		<%= Html.ClientSideValidation(ViewData.Model, "roleform") %>
	<% } %>
</asp:Content>
