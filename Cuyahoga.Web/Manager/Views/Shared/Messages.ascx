<%@ Import Namespace="Cuyahoga.Web.Manager.Model.ViewModels"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MessageViewData>" %>
<div id="messagewrapper" style="display:none">
	<% foreach (string messageType in MessageType.GetTypes()) { %>
		<% if (ViewData.Model.Messages[messageType].Count > 0) { %>
			<div class="<%= messageType.ToLower() %>box">
			<img src="<%= Url.Content("~/Manager/Content/Images/cross.gif") %>" class="close_message" style="float:right;cursor:pointer" alt="Close" />
			<% foreach (string message in ViewData.Model.Messages[messageType]) { %>
				<%= message%><br />
			<% } %>
			</div>
		<% } %>
	<% } %>
</div>
<script type="text/javascript">
	$(document).ready(function(){
		// Display messages on page load if the #messagewrapper has content
		if ($("#messagewrapper").children().length > 0) {
			displayMessages();
		}
	});	
</script>
