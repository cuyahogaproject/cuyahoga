<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Messages.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Shared.Messages" %>
<div id="messagewrapper" style="display:none">
	<% if (ViewData.Model.Messages[MessageType.Exception].Count > 0) { %>
		<p class="errorbox">
		<img src="<%= Url.Content("~/Manager/Content/Images/cross.gif") %>" class="close_message" style="float:right;cursor:pointer" alt="Close" />
		<% foreach (string exceptionMessage in ViewData.Model.Messages[MessageType.Exception]) { %>
			<%= exceptionMessage %><br />
		<% } %>
		</p>
	<% } %>
	<% if (ViewData.Model.Messages[MessageType.Error].Count > 0) { %>
		<p class="errorbox">
		<img src="<%= Url.Content("~/Manager/Content/Images/cross.gif") %>" class="close_message" style="float:right;cursor:pointer" alt="Close" />
		<% foreach (string errorMessage in ViewData.Model.Messages[MessageType.Error]) { %>
			<%= errorMessage%><br />
		<% } %>
		</p>
	<% } %>
	<% if (ViewData.Model.Messages[MessageType.Message].Count > 0) { %>
		<p class="messagebox">
		<img src="<%= Url.Content("~/Manager/Content/Images/cross.gif") %>" class="close_message" style="float:right;cursor:pointer" alt="Close" />
		<% foreach (string message in ViewData.Model.Messages[MessageType.Message]) { %>
			<%= message%><br />
		<% } %>
		</p>
	<% } %>
</div>
<script type="text/javascript">
	$(document).ready(function(){
		addMessageHandlers();
	});	
	function addMessageHandlers() {
		$("#messagewrapper").fadeOut(100).fadeIn(800);
		
		$(".close_message").click(function() { 
			$("#messagewrapper").fadeOut("slow"); 
		});
	}
</script>