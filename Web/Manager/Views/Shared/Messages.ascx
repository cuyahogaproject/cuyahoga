<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Messages.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Shared.Messages" %>
<div id="messagewrapper">
	<% if (ViewData.Model.Messages[MessageType.Exception].Count > 0) { %>
		<div class="errorbox">
		<img src="<%= Url.Content("~/Manager/Content/Images/cross.gif") %>" class="close_message" style="float:right;cursor:pointer" alt="Close" />
		<% foreach (string exceptionMessage in ViewData.Model.Messages[MessageType.Exception]) { %>
			<%= exceptionMessage %><br />
		<% } %>
		</div>
	<% } %>
	<% if (ViewData.Model.Messages[MessageType.Error].Count > 0) { %>
		<div class="errorbox">
		<img src="<%= Url.Content("~/Manager/Content/Images/cross.gif") %>" class="close_message" style="float:right;cursor:pointer" alt="Close" />
		<% foreach (string errorMessage in ViewData.Model.Messages[MessageType.Error]) { %>
			<%= errorMessage%><br />
		<% } %>
		</div>
	<% } %>
	<% if (ViewData.Model.Messages[MessageType.Message].Count > 0) { %>
		<div class="messagebox">
		<img src="<%= Url.Content("~/Manager/Content/Images/cross.gif") %>" class="close_message" style="float:right;cursor:pointer" alt="Close" />
		<% foreach (string message in ViewData.Model.Messages[MessageType.Message]) { %>
			<%= message%><br />
		<% } %>
		</div>
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