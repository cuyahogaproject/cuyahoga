<%@ Page Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Node>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= String.Format(GlobalResources.DesignPagePageTitle, Model.Title) %></title>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.core.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.sortable.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.draggable.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.droppable.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.dialog.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
<%
	bool expandAddNewSection = (bool) (ViewData.ContainsKey("ExpandAddNew") ? ViewData["ExpandAddNew"] : false);
%>
	<h2><%= GlobalResources.TemplateLabel %></h2>
	<% using (Html.BeginForm("SetTemplate", "Pages", FormMethod.Post, new { @id = "templateform" } )) { %>
		<%= Html.Hidden("NodeId", ViewData.Model.Id) %>
		<%= Html.DropDownList("TemplateId", ViewData["Templates"] as SelectList, GlobalResources.ChooseTemplateOption)%>
	<% } %>
	
	<h2><%= GlobalResources.TasksLabel %></h2>
	<%= Html.ActionLink(GlobalResources.ManageContentCurrentPageLabel, "Content", "Pages", new { id = ViewData.Model.Id }, new { @class = "contentlink" }) %>
	<a href="#" class="<%= (expandAddNewSection ? " collapselink" : " expandlink") %>"><%= GlobalResources.AddSectionLabel %></a>
	<div class="taskcontainer"<% if (! expandAddNewSection) { %> style="display:none"<% } %>>
		<p><%= GlobalResources.AvailableModulesHint %></p>
		<ul id="availablemodules">
			<% foreach (ModuleType moduleType in (IEnumerable)ViewData["AvailableModules"]) { %>
				<li id="mt-<%= moduleType.ModuleTypeId %>"><%= moduleType.Name %></li>
			<% } %>
		</ul>
	</div>
	<% using (Html.BeginForm("DeleteSectionFromPage", "Sections", FormMethod.Post, new { id = "deletesectionform" })) { %>
		<%= Html.Hidden("nodeid", ViewData.Model.Id)%>
		<%= Html.Hidden("sectionidtodelete")%>
		<a href="#" class="expandlink"><%= GlobalResources.RemoveSectionLabel %></a>
		<div id="deletebox" class="taskcontainer" style="display:none">
			<p><%= GlobalResources.DeleteSectionBoxHint %></p>
		</div>
	<% } %>		
	
	<div id="selectedsection">
	<% if (ViewData.ContainsKey("ActiveSection")) {
		Section activeSection = (Section)ViewData["ActiveSection"];
		Html.RenderPartial("SelectedSection", activeSection, ViewData);
	} %>
	</div>
	
	<script type="text/javascript">
		var isDeleting = false;
		var isAdding = false;
		<% if (ViewData.ContainsKey("ActiveSection")) { %>
		var selectedSectionId = <%= ((Section)ViewData["ActiveSection"]).Id %>;		
		<% } else { %>
		var selectedSectionId = 0;
		<% } %>
		
		$(document).ready(function() {
			$('#templateform').ajaxForm({ 
				dataType:  'json'
				//success: processJsonMessage // in cuyahoga.common.js
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
			
			ajaxifySelectedSectionForms();
						
			renderSectionsInTemplate();
			
			$('#availablemodules > li').draggable({
				helper: 'clone',
				cursor: 'move',
				connectToSortable: 'ul.sectionlist' 
			});
			
			$('#newsectiondialog').dialog({
				autoOpen: false,
				autoResize: true,
				width: "760px",
				minHeight: "500px",
				buttons: {
					"<%= GlobalResources.CreateSectionLabel %>": createSectionFromDialog, 
					"<%= GlobalResources.CancelLabel %>": closeDialog
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
					"<%= GlobalResources.DetachSectionConfirmLabel %>" : function() {
						$('#deletesectionform').attr('action', '<%= Url.Action("DetachSectionFromPage", "Sections") %>');
						$('#deletesectionform').submit(); 
					},
					"<%= GlobalResources.CancelLabel %>": closeDialog
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
		})
		
		function ajaxifySelectedSectionForms() {
			$('#selectedsection form').ajaxForm({
				target: '#selectedsection',
				success: function() {
					movePartialMessages();
					ajaxifySelectedSectionForms();
					renderSectionsInTemplate();
				}
			});
		}
		
		function renderSectionsInTemplate() {
			$.getJSON('<%= Url.Action("GetSectionsForPage", "Pages") %>', { nodeid:<%= ViewData.Model.Id %> }, function(data) {
				$('.templatecontainer ul.sectionlist').empty();
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
					
						isAdding = true; // Set flag to prevent sortable update.
						
						// Add new section to placeholder 
						var moduleTypeId = $(ui.item).attr('id').substring(3);
						var placeholder = $(this).parent().attr('id').substring(4); // strip 'plh_'
						var newSectionDialogUrl = '<%= Url.Action("NewSectionDialog", "Sections") %>?nodeid=<%= ViewData.Model.Id %>&moduletypeid=' + moduleTypeId + '&placeholder=' + placeholder;

						$('#newsectiondialogcontent').load(newSectionDialogUrl, function(e) {
							$(this).parent().dialog("open");
						});
					}
				},
				update: function(ev, ui) {
					if (! isDeleting && ! isAdding) {
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
			$('.templatecontainer').click($.delegate({
				'li.section-item div': selectSection 
			}))
		}
		
		function createSectionFromDialog(ev, ui) {
			$('#newsectiondialog form').ajaxSubmit({
				target: '#newsectiondialogcontent',
				success: function() {
					if ($('#newsectiondialog #isvalid').val() == 'True') {
						movePartialMessages();
						$('#newsectiondialog').dialog('close');
						renderSectionsInTemplate();
					}
				}
			});
		}
		
		function closeDialog(ev, ui) {
			// reload page to prevent sorting.
			document.location.href = '<%= Url.Action("Design", "Pages", new { id = ViewData.Model.Id }) %>'; 
		}
		
		function selectSection(e) {
			var selectedSectionElement = $(e.target).is('div') ? $(e.target).parents('.section-item') : $(e.target);
			
			if (selectedSectionElement.length > 0) {
				$('#section-' + selectedSectionId).removeClass('section-selected'); // remove previous selected section.
				selectedSectionId = selectedSectionElement.attr('id').substring(8); // strip 'section-'
				selectedSectionElement.addClass('section-selected');
				$('#selectedsection').unbind();
				$('#selectedsection').load('<%= Url.Action("SelectSection", "Sections") %>', { sectionid : selectedSectionId }, ajaxifySelectedSectionForms);
			}
		}
		
		function createSectionItemElement(sectionId, sectionName, moduleType, editUrl) {
			var el = '<li id="section-{0}" class="{3}"><div>{1} ({2})</div></li>';
			var sectionItemClass = 'section-item';
			if (sectionId == selectedSectionId) {
				sectionItemClass += ' section-selected';
			}
			return jQuery.validator.format(el, sectionId, sectionName, moduleType, sectionItemClass);
		}

	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= String.Format(GlobalResources.DesignPagePageTitle, Model.Title) %></h1>
	<div class="templatecontainer">
	<% if (ViewData.ContainsKey("TemplateViewData")) { %>
		<% Html.RenderPartial("PageTemplate", ViewData["TemplateViewData"]); %>
	<% } %>
	</div>
	<p><%= Html.ActionLink(GlobalResources.BackToPageListLabel, "Index", new { id = ViewData.Model.Id }) %></p>
	
	<div id="newsectiondialog" title="<%= GlobalResources.AddSectionDialogTitle %>">
		<div id="newsectiondialogcontent" class="dialog-content">
		
		</div>
	</div>
	
	<div id="deletesectiondialog" title="<%= GlobalResources.RemoveSectionDialogTitle %>">
		<p class="dialog-content"><%= GlobalResources.DeleteSectionChoiceLabel %></p>
	</div>
		
</asp:Content>
