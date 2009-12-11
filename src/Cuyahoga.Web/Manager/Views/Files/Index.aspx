<%@ Page Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<DirectoryViewData>" %>
<%@ Import Namespace="Cuyahoga.Web.Manager.Model.ViewModels"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.ManageFilesPageTitle %></title>
	<%= Html.ScriptInclude("~/manager/Scripts/swfobject.js") %>
	<%= Html.ScriptInclude("~/manager/Scripts/jquery.uploadify.v2.0.3.min.js") %>
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
					fileListLoaded();
				});
				return false;
			});
			// make sure the forms in the filemanager div only refresh the content of that div (when posting)
			ajaxifyFileManagerForms();

			// load the initial file list
			loadFilesForPath('<%= Model.Path %>');

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

			// select row in grid
			$('#filemanager table.grid tr')
			.filter(':has(:checkbox:checked)')
			.addClass('selected')
			.end()
			.live('click', function(event) {
				$(this).toggleClass("selected");

				if (event.target.type !== "checkbox") {
					checkbox = $(":checkbox", this);
					checkbox.attr("checked", checkbox.is(':not(:checked)'));
				}
				if ($(this).parent().find('tr:has(:checkbox:checked)').length > 0) {
					$('#fileactions').show();
				}
				else {
					$('#fileactions').hide();
				}
			});
		});

		function fileListLoaded() {
			ajaxifyFileManagerForms();
			loadAvailableDirectories();
			applyUploadify();
		}

		function ajaxifyFileManagerForms() {
			$('#filemanager form').ajaxForm({
				target: '#filemanager',
				success: function() {
					fileListLoaded();
					movePartialMessages();
				}
			});
		}

		function loadFilesForPath(path) {
			$('#filemanager').load('<%= Url.Action("List", "Files") %>', { 'path': path }, function() {
				fileListLoaded();
			});
		}

		function loadAvailableDirectories() {
			$.getJSON('<%= Url.Action("GetAllDirectories", "Files") %>', function(data) {
				$.each(data, function(i, item) {
					$('#pathto').append('<option value="' + item.Path + '">' + getIndent(item.Level) + item.Name + '</option>');
				});
			});
		}

		function applyUploadify() {
			if (swfobject.getFlashPlayerVersion().major > 8) {
				var auth = "<% = Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value %>";
				var uploadPath = $('#uploadpath').val();
				$('#filedata').uploadify({
					'fileDataName': 'filedata',
					'uploader': '<%= Url.Content("~/manager/Scripts/uploadify.swf") %>',
					'script': '<%= Url.Action("UploadAjax", "FilesUpload") %>',
					'scriptData': {
						'uploadpath': uploadPath,
						'token': auth
					},
					'scriptAccess': 'always',
					'cancelImg': '<%= Url.Content("~/manager/Content/Images/cancel.png") %>',
					'multi': true,
					'auto': false,
					'buttonText': '<%= GlobalResources.BrowseButtonLabel %>',
					'onAllComplete': function(event, data) {
						// reload files
						loadFilesForPath(uploadPath);
					}
				});
				$('#uploadbutton').click(function() {
					$('#filedata').uploadifyUpload();
					return false;
				});
			}
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
