<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Import Namespace="Cuyahoga.Web.Components"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Shared.Header" %>
<div id="header">
	<img height="60" width="172" alt="Cuyahoga logo" src="<%= Url.Content("~/Manager/Content/Images/cuyahoga-logo.png") %>"/>
	<% if (ViewData.ContainsKey("CuyahogaUser")) { %>
	<div id="userinfo">
		<%= GlobalResources.LoggedInAsLabel %> <%= ((User)ViewData["CuyahogaUser"]).FullName %><br />
		<%= Html.ActionLink(GlobalResources.LogoutButtonLabel, "Logout", "Login") %>
	</div>
	<% } %>
	<p><%= GlobalResources.CuyahogaSiteManagerLabel %></p>
</div>