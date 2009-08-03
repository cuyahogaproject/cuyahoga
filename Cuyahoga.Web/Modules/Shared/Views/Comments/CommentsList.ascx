<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Comment>>" %>
<h3><%= GlobalResources.CommentsLabel %></h3>
<table class="grid">
	<thead>
		<tr>
			<th><%= GlobalResources.AuthorLabel %></th>
			<th><%= GlobalResources.CommentLabel %></th>
			<th>&nbsp;</th>
		</tr>
	</thead>
	<tbody>
		<% foreach (var comment in Model) { %>
			<tr>
				<td>
					<em><strong><%= comment.AuthorName %></strong></em><br />
					<% if (! String.IsNullOrEmpty(comment.WebSite)) {%>
						<a href="<%= comment.WebSite %>"><%= comment.WebSite %></a><br />
					<% }%>
					<%= comment.UserIp %>
				</td>
				<td>
					<b><%= comment.CommentDateTime %></b><br />
					<%= Server.HtmlEncode(comment.CommentText) %>
				</td>
				<td>
					<% using (Html.BeginForm("DeleteCommentForContentItem", "Comments", new { contentitemid = comment.ContentItem.Id, id = comment.Id }, FormMethod.Post)) { %>
						<a href="#" class="deletelink"><%= GlobalResources.DeleteButtonLabel %></a>
					<% } %>
				</td>
			</tr>
		<% } %>
	</tbody>
</table>