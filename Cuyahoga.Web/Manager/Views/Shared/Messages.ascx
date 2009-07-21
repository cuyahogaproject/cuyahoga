<%@ Import Namespace="Cuyahoga.Web.Mvc.ViewModels"%>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MessageViewData>" %>
<div id="messagewrapper" style="display:none">
	<% 
	var messages = Model.GetDisplayMessages(); 
	foreach (string messageType in MessageType.GetTypes()) {
		if (messages[messageType].Count > 0) { %>
			<div class="<%= messageType.ToLower() %>box">
			<img src="<%= Url.Content("~/Manager/Content/Images/cross.gif") %>" class="close_message" style="float:right;cursor:pointer" alt="Close" />
			<% foreach (string message in messages[messageType]) { %>
				<%= message%><br />
			<% } %>
			</div>
		<% } %>
	<% } %>
</div>
<script type="text/javascript">
	$(document).ready(function() {
		// Display messages on page load if the #messagewrapper has content
		if ($("#messagewrapper").children().length > 0) {
			displayMessages();
		}
	});	
</script>
