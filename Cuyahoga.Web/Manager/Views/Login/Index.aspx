<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Default.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Login.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<style type="text/css">
	body
	{
		background-color: #eef;
	}
	#loginpanel
	{
		width: 350px;
		height: 180px;
		background-color: #fff;
		margin-top: 100px;
		margin-left: auto;
		margin-right: auto;
		padding: 10px;
		border-top: 1px solid #ccc;
		border-left: 1px solid #ccc;
		border-bottom: 1px solid #ccc;
		border-right: 8px solid #ccc;	
	}

	fieldset
	{
		border: none;
	}

	fieldset label 
	{  
		width: 100px;  
	}
	
	fieldset li
	{
		border: none;
	}

	fieldset label.error
	{
		margin-left: 110px;
	}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
	<div id="loginpanel">
		<h1><%= GlobalResources.LoginPageTitle %></h1>
		<% using (Html.BeginForm("Login", "Login", FormMethod.Post, new { id = "loginform" })) { %>
			<%= Html.Hidden("ReturnUrl", ViewData["ReturnUrl"]) %>
			<fieldset>
			<ol>
				<li>
				<label for="Username"><%= GlobalResources.UsernameLabel %></label>
				<%= Html.TextBox("Username") %>
				</li>
				<li>
				<label for="Password"><%= GlobalResources.PasswordLabel %></label>
				<%= Html.Password("Password") %>
				</li>
			</ol>
			</fieldset>
			<%= Html.ClientSideValidation(ViewData.Model, "loginform") %>
			<input type="submit" value="Login" />
		<% } %>
	</div>
</asp:Content>
