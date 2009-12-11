<%@ control language="C#" autoeventwireup="true" codebehind="InlineEditing.ascx.cs"	inherits="Cuyahoga.Web.Controls.InlineEditing" %>
<%@ Import Namespace="Resources.Cuyahoga.Web.Manager"%>
<script type="text/javascript">
	$(document).ready(function() {
		$('div.moduletools > a').click(function(e) {
			e.preventDefault();
			var $this = $(this);
			var horizontalPadding = 20;
			var verticalPadding = 20;
			$('<iframe id="editdialog" frameborder="0" src="' + this.href + '" />').dialog({
				title: ($this.attr('title')) ? $this.attr('title') : 'Edit',
				autoOpen: true,
				width: 760,
				height: 500,
				buttons: {
					"<%= GlobalResources.CloseLabel %>": closeDialog
				}, 
				modal: true,
				resizable: true,
				autoResize: true,
				overlay: {
					opacity: 0.5,
					background: "black"
				},
				close: closeDialog
			}).width(760 - horizontalPadding).height(500 - verticalPadding);
		});
	});

	function closeDialog(ev, ui) {
		// reload page to prevent sorting.
		document.location.href = '<%= Request.RawUrl %>';
	}
</script>

