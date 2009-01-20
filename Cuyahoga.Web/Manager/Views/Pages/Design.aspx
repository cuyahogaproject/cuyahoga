<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="Design.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.Design" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Manager/Content/Css/jquery-ui/ui.dialog.css") %>" />
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/jquery.form.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.core.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.sortable.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.draggable.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.droppable.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.dialog.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TemplateLabel %></h2>
	<% using (Html.BeginForm("SetTemplate", "Pages", FormMethod.Post, new { @id = "templateform" } )) { %>
		<%= Html.Hidden("NodeId", ViewData.Model.Id) %>
		<%= Html.DropDownList(GlobalResources.ChooseTemplateOption, "TemplateId", ViewData["Templates"] as SelectList)%>
	<% } %>
	<h2><%= GlobalResources.AvailableModulesLabel %></h2>
	<div class="taskcontainer">
		<p><%= GlobalResources.AvailableModulesHint %></p>
		<ul id="availablemodules">
			<% foreach (ModuleType moduleType in (IEnumerable)ViewData["AvailableModules"]) { %>
				<li id="mt-<%= moduleType.ModuleTypeId %>"><%= moduleType.Name %></li>
			<% } %>
		</ul>
	</div>
	<h2><%= GlobalResources.RemoveSectionLabel %></h2>
	<% using (Html.BeginForm("DeleteSectionFromPage", "Sections", FormMethod.Post, new { id = "deletesectionform" })) { %>
		<%= Html.Hidden("nodeid", ViewData.Model.Id)%>
		<%= Html.Hidden("sectionidtodelete")%>
		<div id="deletebox" class="taskcontainer">
			<p></p>
		</div>
	<% } %>		
	<script type="text/javascript">
		var isDeleting = false;
		
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
				cursor: 'move',
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
			
			$('#sectionpropertiesdialog').dialog({
				autoOpen: false,
				width: "760px",
				height: "500px",
				buttons: {
					"<%= GlobalResources.CloseLabel %>": closeDialog
				}, 
				modal: true,
				overlay: { 
					opacity: 0.5, 
					background: "black" 
				}
			})
			
			$('#editcontentdialog').dialog({
				autoOpen: false,
				width: "800px",
				height: "560px",
				buttons: {
					"<%= GlobalResources.CloseLabel %>": closeDialog
				}, 
				modal: true,
				overlay: { 
					opacity: 0.5, 
					background: "black" 
				}
			})
			
			$('#deletesectiondialog').dialog({
				autoOpen: false,
				width: "520px",
				height: "150px",
				buttons: { 
					"<%= GlobalResources.DeleteSectionConfirmLabel %>": function() { 
						$('#deletesectionform').attr('action', '<%= Url.Action("DeleteSectionFromPage", "Sections") %>');
						$('#deletesectionform').submit(); 
					},
					"<%= GlobalResources.DetachSectionConfirmLabel %> : function() {
						$('#deletesectionform').attr('action', '<%= Url.Action("DetachSectionFromPage", "Sections") %>');
						$('#deletesectionform').submit(); 
					},
					"<%= GlobalResources.CancelLabel %>": closeDialog,
				}, 
				modal: true,
				overlay: { 
					opacity: 0.5, 
					background: "black" 
				},
				close: closeDialog 
			});			
			
			$('#deletebox').droppable({
				accept: "li.section-item",
				hoverClass: "drophover",
				drop: function(ev, ui) {
					isDeleting = true;
					var sectionIdToDelete = ui.draggable.attr('id').substring(8);
					$('#sectionidtodelete').val(sectionIdToDelete);
					$('#deletesectiondialog').dialog("open");
				}
			})
			
			$('.templatecontainer').click($.delegate({
				'img.sectionpropertieslink': function(e) {
					$('#sectionpropertiesframe').attr('src', $(e.target).parent().attr("href"));
					$('#sectionpropertiesdialog').dialog("open");
					return false; 
				},
				'img.editcontentlink': function(e) { 
					$('#editcontentframe').attr('src', $(e.target).parent().attr("href"));
					$('#editcontentdialog').dialog("open");
					return false;
				}
			}))
		})
		
		function renderSectionsInTemplate() {
			$.getJSON('<%= Url.Action("GetSectionsForPage", "Pages") %>', { nodeid:<%= ViewData.Model.Id %> }, function(data) {
				$.each(data, function(i, item) {
					var sectionsSelector = '#plh-' + item.PlaceHolder + ' > ul';
					if ($(sectionsSelector).length == 0) {
						sectionsSelector = '#plh-unknown > ul';
					}
					$(sectionsSelector).append(createSectionItemElement(item.SectionId, item.SectionName, item.ModuleType, item.EditUrl));
				})
			});
			$('.sectionlist').sortable({
				opacity: "0.5",
				placeholder: "sectionplaceholder",
				connectWith: ['ul.sectionlist'],
				dropOnEmpty: true,
				receive: function (ev, ui) {
					if ($(ui.item).attr('id').substring(0,3) == "mt-") {
						// Add new section to placeholder 
						var moduleTypeId = $(ui.item).attr('id').substring(3);
						var placeholder = $(this).parent().attr('id').substring(4); // strip 'plh_'
						var newSectionDialogUrl = '<%= Url.Action("NewSectionDialog", "Sections") %>?nodeid=<%= ViewData.Model.Id %>&moduletypeid=' + moduleTypeId + '&placeholder=' + placeholder;

						$('#newsectionpropertiesframe').attr('src', newSectionDialogUrl);
						$('#newsectiondialog').dialog("open");
					}
				},
				update: function(ev, ui) {
					if (! isDeleting) {
						var serializedChildNodeIds = $(this).sortable('serialize');
						// we only need an array of section id's, extract these with a regex.
						var orderedSectionIds = serializedChildNodeIds.match(/(\d+)/g);
						var placeholder =  $(this).parent().attr('id').substring(4); // strip 'plh_'
						$.post('<%= Url.Action("ArrangeSections", "Sections") %>',
							{ placeholder : placeholder, orderedSectionIds : orderedSectionIds },
							processJsonMessage,
							"json");
					}
				}
			})
		}
		
		function createSectionFromDialog(ev, ui) {
			$('#newsectionpropertiesframe').contents().find('form').submit(); 
		}
		
		function closeDialog(ev, ui) {
			// reload page to prevent sorting.
			document.location.href = '<%= Url.Action("Design", "Pages", new { id = ViewData.Model.Id }) %>'; 
		}
		
		function createSectionItemElement(sectionId, sectionName, moduleType, editUrl) {
			var el = '<li id="section-{0}" class="section-item"><div style="float:right">{3}</div><div>{1} ({2})</div></li>';
			var sectionLinks = jQuery.format('<a href="<%= Url.Action("SectionProperties", "Sections") %>/{0}" title="<%= GlobalResources.SectionPropertiesLabel %>"><img src="<%= Url.Content("~/manager/Content/Images/application_form.png") %>" alt="<%= GlobalResources.SectionPropertiesLabel %>" class="sectionpropertieslink" /></a>', sectionId);
			if (editUrl != '') {
				editLink = jQuery.format('<a href="<%= ResolveUrl("~/") %>{0}?nodeid={1}&sectionid={2}" title="<%= GlobalResources.EditContentLabel %>"><img src="<%= Url.Content("~/manager/Content/Images/pencil.png") %>" alt="<%= GlobalResources.EditContentLabel %>" class="editcontentlink" /></a>', editUrl, $('#NodeId').val(), sectionId);
				sectionLinks = editLink + ' ' + sectionLinks;
			}
			return jQuery.format(el, sectionId, sectionName, moduleType, sectionLinks);
		}

	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<div class="templatecontainer">
	<% if (ViewData.ContainsKey("TemplateViewData")) { %>
		<% Html.RenderPartial("PageTemplate", ViewData["TemplateViewData"]); %>
	<% } %>
	</div>
	<div id="newsectiondialog" title="<%= GlobalResources.AddSectionDialogTitle %>">
		<iframe id="newsectionpropertiesframe" class="dialog-content" style="width:740px;height:400px"></iframe>
	</div>

	<div id="sectionpropertiesdialog" title="<%= GlobalResources.SectionPropertiesDialogTitle %>">
		<iframe id="sectionpropertiesframe" class="dialog-content" style="width:740px;height:400px"></iframe>
	</div>
	
	<div id="deletesectiondialog" title="<%= GlobalResources.RemoveSectionDialogTitle %>">
		<p class="dialog-content">Do you want to delete the entire section or only detach it from the page?</p>
	</div>
		
	<div id="editcontentdialog" title="<%= GlobalResources.EditContentDialogTitle %>">
		<iframe id="editcontentframe" class="dialog-content" style="width:780px;height:460px"></iframe>
	</div>
	

</asp:Content>
