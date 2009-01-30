<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PartialMessages.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Shared.PartialMessages" %>
<div id="partialmessagewrapper" style="display:none">
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
	if ($('#partialmessagewrapper').children().length > 0 && $('#messagewrapper').length > 0) {
		$('#messagewrapper').empty();
		$('#messagewrapper').append($('#partialmessagewrapper').children());
		displayMessages();
	}
</script>