<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ContentItem>" %>
<h3><%= GlobalResources.CommentsLabel %></h3>
<div id="comments">
</div>
<script type="text/javascript">
	$(document).ready(function() {
		$('#comments').load('<%= Url.Content("~/Modules/Shared/Comments/ViewByContentItem") %>'
			, { contentitemid: <%= Model.Id %> }
			, ajaxifyCommentForms);
	});

	function ajaxifyCommentForms() {
		$('#comments form').ajaxForm({
		target: '#comments',
			success: ajaxifyCommentForms
		});
	}
</script>