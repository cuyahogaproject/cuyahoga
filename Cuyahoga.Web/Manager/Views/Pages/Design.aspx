<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="Design.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.Design" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Manager/Content/Css/jquery-ui/ui.dialog.css") %>" />
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/jquery.form.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.core.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.sortable.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.draggable.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.dialog.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TemplateLabel %></h2>
	<% using (Html.BeginForm("SetTemplate", "Pages", FormMethod.Post, new { @id = "templateform" } )) { %>
		<%= Html.Hidden("NodeId", ViewData.Model.Id) %>
		<%= Html.DropDownList(GlobalResources.ChooseTemplateOption, "TemplateId", ViewData["Templates"] as SelectList)%>
	<% } %>
	<ul id="availablemodules">
		<% foreach (ModuleType moduleType in (IEnumerable)ViewData["AvailableModules"]) { %>
			<li id="mt-<%= moduleType.ModuleTypeId %>"><%= moduleType.Name %></li>
		<% } %>
	</ul>
	
	<div id="newsectiondialog" title="<%= GlobalResources.AddSectionDialogTitle %>">
		<iframe id="sectionproperties" class="dialog-content" style="width:740px;height:400px"></iframe>
	</div>
	
	<script type="text/javascript">
		$(document).ready(function() {
			$('#templateform').ajaxForm({ 
				dataType:  'json' 
				// Success messages conflict with sortables success:   processJsonMessage // in cuyahoga.common.js
			}); 
			
			$('#TemplateId').change(function() {
				var templateId = $('#TemplateId').val();
				if (templateId > 0) {
					// First, save template
					$('#templateform').submit();
					// Load template into container.
					$('.templatecontainer').load('<%= Url.Action("ShowTemplate", "Pages") %>', { templateid: templateId }, function() {
						renderSectionsInTemplate();
					});
				}
			})
			
			renderSectionsInTemplate();
			$('#availablemodules > li').draggable({
				helper: 'clone',
				connectToSortable: 'ul.sectionlist' 
			});
			
			$('#newsectiondialog').dialog({
				autoOpen: false,
				width: "760px",
				height: "500px",
				buttons: {
					"<%= GlobalResources.CreateSectionLabel %>": createSectionFromDialog, 
					"<%= GlobalResources.CancelLabel %>": closeDialog
				}, 
				modal: true,
				overlay: { 
					opacity: 0.5, 
					background: "black" 
				},
				close: closeDialog 
			})
		})
		
		function renderSectionsInTemplate() {
			$.getJSON('<%= Url.Action("GetSectionsForPage", "Pages") %>', { nodeid:<%= ViewData.Model.Id %> }, function(data) {
				$.each(data, function(i, item) {
					var sectionsSelector = '#plh-' + item.PlaceHolder + ' > ul';
					$(sectionsSelector).append('<li>' + item.SectionName + ' (' + item.ModuleType + ')</li>');
				})
			});
			$('.sectionlist').sortable({
				opacity: 0.5,
				placeholder: "sectionplaceholder",
				connectWith: ['ul.sectionlist'],
				dropOnEmpty: true,
				receive : function (ev, ui) {
					if ($(ui.item).attr('id').substring(0,3) == "mt-") {
						// Add new section to placeholder 
						var moduleTypeId = $(ui.item).attr('id').substring(3);
						var placeholder = $(this).parent().attr('id').substring(4); // strip 'plh_'
						var newSectionDialogUrl = '<%= Url.Action("NewSectionDialog", "Sections") %>?nodeid=<%= ViewData.Model.Id %>&moduletypeid=' + moduleTypeId + '&placeholder=' + placeholder;

						$('#sectionproperties').attr('src', newSectionDialogUrl);
						$('#newsectiondialog').dialog("open");
					}
					else {
						// 'Regular' sorting
					}
				} 
			})
		}
		
		function createSectionFromDialog(ev, ui) {
			$('#sectionproperties').contents().find('form').submit(); 
		}
		
		function closeDialog(ev, ui) {
			// reload page to prevent sorting.
			document.location.href = '<%= Url.Action("Design", "Pages", new { id = ViewData.Model.Id }) %>'; 
		}

	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<div class="templatecontainer">
	<% if (ViewData.ContainsKey("TemplateViewData")) { %>
		<% Html.RenderPartial("PageTemplate", ViewData["TemplateViewData"]); %>
	<% } %>
	</div>
</asp:Content>
