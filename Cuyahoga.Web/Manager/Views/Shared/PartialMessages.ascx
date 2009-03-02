<%@ Import Namespace="Cuyahoga.Web.Mvc.ViewModels"%>

<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MessageViewData>" %>
<div class="partialmessagewrapper">
	<% 	
	var messages = ViewData.Model.GetDisplayMessages(); 
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