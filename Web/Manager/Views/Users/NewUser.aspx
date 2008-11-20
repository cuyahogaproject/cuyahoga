<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="NewUser.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Users.NewUser" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<%= Html.ValidationSummary() %>	
	<% using(Html.BeginForm("Create", "Users", FormMethod.Post, new { id = "userform" })) { %>
		<fieldset>
			<legend><%=GlobalResources.GeneralLabel%></legend>
			<ol>
				<li>
					<label for="UserName"><%=GlobalResources.UsernameLabel%></label>
					<%=Html.TextBox("UserName", ViewData.Model.UserName)%>
				</li>
				
				<% Html.RenderPartial("SharedUserFormElements", ViewData.Model, ViewData); %>
				
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
		
		<% Html.RenderPartial("RoleSelector", ViewData.Model, ViewData); %>
		
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
		<%= GlobalResources.Or %>
		<%= Html.ActionLink(GlobalResources.CancelLabel, "Index", "Users") %>
		<%= Html.ClientSideValidation(ViewData.Model, "userform") %>
	<% } %>
</asp:Content>
