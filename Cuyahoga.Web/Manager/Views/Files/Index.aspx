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
				$('#filemanager').load($(this).attr('href'));
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
					}
				});
				return false;
			});
			// load the initial file list
			$('#filemanager').load('<%= Url.Action("List", "Files", new { path = Model.Path } )%>');
		});
	</script>
</asp:Content>
