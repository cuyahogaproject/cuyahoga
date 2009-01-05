<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Users.EditUser" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<% using(Html.BeginForm("Update", "Users", new { id = ViewData.Model.Id }, FormMethod.Post, new { id = "userform" })) { %>
		<fieldset>
			<legend><%=GlobalResources.GeneralLabel%></legend>
			<ol>
				<li>
					<label for="UserName"><%=GlobalResources.UsernameLabel%></label>
					<%=ViewData.Model.UserName%>
				</li>
				<% Html.RenderPartial("SharedUserFormElements", ViewData.Model, ViewData); %>
			</ol>
		</fieldset>
		<% Html.RenderPartial("RoleSelector", ViewData.Model, ViewData); %>
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
		<%= GlobalResources.Or %>
		<%= Html.ActionLink(GlobalResources.CancelLabel, "Index", "Users") %>
		<%= Html.ClientSideValidation(ViewData.Model, "userform")%>
	<% } %>
	<br />
	<% using (Html.BeginForm("ChangePassword", "Users", new { id = ViewData.Model.Id }, FormMethod.Post, new { id = "passwordform" })) { %>
		<fieldset>
			<legend><%=GlobalResources.ChangePasswordLabel%></legend>
			<ol>
				<li>
					<label for="Password"><%=GlobalResources.PasswordLabel%></label>
					<%=Html.Password("Password", String.Empty)%>
				</li>
				<li>
					<label for="PasswordConfirmation"><%=GlobalResources.ConfirmPasswordLabel%></label>
					<%=Html.Password("PasswordConfirmation", String.Empty)%>
				</li>
			</ol>
		</fieldset>
		<input type="submit" value="<%= GlobalResources.ChangePasswordLabel %>" />
		<%= GlobalResources.Or %>
		<%= Html.ActionLink(GlobalResources.CancelLabel, "Index", "Users") %>
		<%= Html.ClientSideValidation(ViewData.Model, "passwordform")%>
	<% } %>
</asp:Content>
