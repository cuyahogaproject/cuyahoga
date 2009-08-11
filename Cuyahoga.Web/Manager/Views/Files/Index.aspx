<%@ Page Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<DirectoryViewData>" %>
<%@ Import Namespace="Cuyahoga.Web.Manager.Model.ViewModels"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.ManageFilesPageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= GlobalResources.ManageFilesPageTitle %></h1>
	
	<div id="filemanager">
	
	</div>
	<script type="text/javascript">
		$(document).ready(function() {
			// make sure the links in the filemanager div only reload the content of that div.
			$('#filemanager a').live("click", function() {
				$('#filemanager').load($(this).attr('href'), function() {
					loadAvailableDirectories();
				});
				return false;
			});
			// make sure the forms in the filemanager div only refresh the content of that div (when posting)
			$('#filemanager form').live('submit', function() {
				var formdata = $(this).serialize();
				$.ajax({
					type: $(this).attr('method'),
					url: $(this).attr('action'),
					data: formdata,
					success: function(data) {
						$('#filemanager').html(data);
						loadAvailableDirectories();
						movePartialMessages();
					}
				});
				return false;
			});
			// load the initial file list
			$('#filemanager').load('<%= Url.Action("List", "Files", new { path = Model.Path } )%>', function() {
				// load the available directories
				loadAvailableDirectories();
			});

			// change handler for the action select
			$('#fileaction').live('change', function() {
				var theAction = $(this).val();
				$('#fileactionform').attr('action', theAction);
				if (theAction.indexOf('/Delete') > 0) {
					$('#pathto').hide();
				}
				else {
					$('#pathto').show();
				}
			});

			// confirmation
			$('#fileactionbutton').live('click', function() {
				return confirm('<%= GlobalResources.AreYouSure %>');
			});
		});

		function loadAvailableDirectories() {
			$.getJSON('<%= Url.Action("GetAllDirectories", "Files") %>', function(data) {
				$.each(data, function(i, item) {
					$('#pathto').append('<option value="' + item.Path + '">' + getIndent(item.Level) + item.Name + '</option>');
				});
			});
		}

		function getIndent(level) {
			var indent = "";
			for (i = 0; i < level; i++) {
				indent += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
			}
			return indent;
		}
	</script>
</asp:Content>
