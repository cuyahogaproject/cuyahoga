<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Messages.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Shared.Messages" %>
<% if (ViewData.Model.Messages[MessageType.Exception].Count > 0) { %>
	<p class="errorbox">
	<% foreach (string exceptionMessage in ViewData.Model.Messages[MessageType.Exception]) { %>
		<%= exceptionMessage %><br />
	<% } %>
	</p>
<% } %>
<% if (ViewData.Model.Messages[MessageType.Error].Count > 0) { %>
	<p class="errorbox">
	<% foreach (string errorMessage in ViewData.Model.Messages[MessageType.Error]) { %>
		<%= errorMessage%><br />
	<% } %>
	</p>
<% } %>
<% if (ViewData.Model.Messages[MessageType.Message].Count > 0) { %>
	<p class="messagebox">
	<% foreach (string message in ViewData.Model.Messages[MessageType.Message]) { %>
		<%= message%><br />
	<% } %>
	</p>
<% } %>
