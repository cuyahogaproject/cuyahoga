<%@ Page Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IList<Node>>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.ManagePagesPageTitle %></title>
	<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Manager/Content/Css/Pagegrid.css") %>" />
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.core.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.sortable.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.droppable.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.dialog.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/jquery.scrollfollow.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<div id="tasks">
	<% Html.RenderPartial("Tasks", ViewData["ActiveNode"] ?? new Node(), ViewData); %>
	</div>
	<div id="selectedpage">
	<% if (ViewData["ActiveNode"] != null) {
		Node activeNode = (Node) ViewData["ActiveNode"];
		if (activeNode.IsExternalLink)
		{
			Html.RenderPartial("SelectedLink", activeNode, ViewData);
		}
		else
		{
			Html.RenderPartial("SelectedPage", activeNode, ViewData);
		}
	} %>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= GlobalResources.ManagePagesPageTitle%></h1>
	<% using (Html.BeginForm("MovePage", "Pages", FormMethod.Post, new { id = "pagesform" })) { %>
	<%= Html.Hidden("nodeid") %>
	<%= Html.Hidden("newparentnodeid") %>
	<div id="pagegrid">
		<div id="pagegrid-head">
			<div class="fr" style="width:120px"><%= GlobalResources.LastModifiedLabel %></div>
			<div class="fr" style="width:80px"><%= GlobalResources.CultureLabel %></div>
			<div class="fr" style="width:120px"><%= GlobalResources.TemplateLabel %></div>
			<div class="fr" style="width:160px"><%= GlobalResources.PageUrlLabel %></div>
			<div><%= GlobalResources.PageTitleLabel %></div>
		</div>
		<div id="pagegrid-body">
			<% Html.RenderPartial("PageListItems", ViewData.Model, ViewData); %>
		</div>
	</div>
	<div id="movedialog" title="<%= GlobalResources.MoveOrCopyPageDialogTitle %>">
		<p class="dialog-content"><%= GlobalResources.MoveOrCopyPageDialogText %></p>
	</div>
	<% } %>
	<script type="text/javascript"> 
		var selectedPageItem;
		var isMoving = false;
		
		$(document).ready(function() {
			
			$('#taskarea').scrollFollow({
				container: 'contentarea'
			});
			
			$('#pagegrid').click($.delegate({
				'.children-visible': function(e) { 
					toggleHide(e.target); 
				},
				'.children-hidden': function(e) { 
					toggleShow(e.target);
				},
				'.pagerow div': function(e) {
					selectPage(e.target);
				},
				'span': function(e) {
					selectPage(e.target);
				}
			}))
			
			$('#pagegrid').dblclick($.delegate({
				'.pagerow div': function (e) {
					selectedPageItem = $(e.target).parents('.pagerow').parent();
					var nodeId = selectedPageItem.attr('id').substring(5);
					document.location.href = '<%= Url.Action("Design", "Pages") %>/' + nodeId;
				}
			}))
			
			addDroppable('.page');
			addSortable('.pagegroup .pagegroup');
					
			selectedPageItem = $('#pagegrid div.selected').parent();
			
			$('#movedialog').dialog({
				autoOpen: false,
				buttons: { 
					"<%= GlobalResources.MoveButtonLabel %>": function() { 
						$('#pagesform').attr('action', '<%= Url.Action("MovePage", "Pages") %>');
						$('#pagesform').submit(); 
					},
					"<%= GlobalResources.CopyButtonLabel %>": function() {
						$('#pagesform').attr('action', '<%= Url.Action("CopyPage", "Pages") %>');
						$('#pagesform').submit(); 
					},
					"<%= GlobalResources.CancelLabel %>": function() {
						$(this).dialog("close");
					}
				}, 
				modal: true,
				overlay: { 
					opacity: "0.5", 
					background: "black" 
				},
				close: closeDialog 
			});			
		})	
		
		function closeDialog(ev, ui) {
			// cancel sorting after doing something with the dialog.
			$('pagegroup').sortable("cancel");
		}
				
		function toggleHide(expander) {
			$(expander).attr('src', '<%= Url.Content("~/manager/Content/Images/expand.png") %>');
			$(expander).removeClass('children-visible').addClass('children-hidden');
			var nodeId = $(expander).parents('.pagerow').parent().attr('id').substring(5);
			hidePages(nodeId);
		}
		
		function toggleShow(expander) {
			$(expander).attr('src', '<%= Url.Content("~/manager/Content/Images/collapse.png") %>');
			$(expander).removeClass('children-hidden').addClass('children-visible');
			var nodeId = $(expander).parents('.pagerow').parent().attr('id').substring(5);
			showPages(nodeId);	
		}
		
		function hidePages(parentNodeId) {
			$('.parent-' + parentNodeId).hide().each(function(i) {
				hidePages($(this).attr('id').substring(5));
			});
		}
		
		function showPages(parentNodeId) {
			if ($('.parent-' + parentNodeId).length == 0) {
				$.get('<%= Url.Action("GetChildPageListItems", "Pages") %>', { 'nodeid' : parentNodeId }, function(data) {
					$('#page-' + parentNodeId).append(data);
					addSortable('#page-' + parentNodeId + ' .pagegroup');
					addDroppable('#page-' + parentNodeId + ' .page')
				})
			}
			else {
				$('.parent-' + parentNodeId).show();
				// only recurse pages that have their children visible
				$('.parent-' + parentNodeId + ':has(img.children-visible)').each(function(i) {
					showPages($(this).attr('id').substring(5));
				});
			}
		}
		
		function selectPage(pageCell) {
			$('#pagegrid .selected').removeClass('selected');
			
			selectedPageItem = $(pageCell).parents('.pagerow').parent();
			var nodeId = selectedPageItem.attr('id').substring(5);
			$('#selectedpage').load('<%= Url.Action("SelectPage", "Pages") %>', { 'nodeid' : nodeId });
			$('#tasks').load('<%= Url.Action("RefreshTasks", "Pages") %>', { 'nodeid' : nodeId });
			selectedPageItem.find('.pagerow:first').addClass('selected');
		}
		
		function addSortable(container) {
			$(container).sortable({
				opacity: 0.5,
				placeholder: "pageplaceholder",
				delay: 50,
				distance: 20,
				update: function(ev, ui) {
					if (! isMoving) {
						var parentNodeId = ui.item.attr('class').substring(7);
						var serializedChildNodeIds = $(this).sortable('serialize');
						// we only need an array of node id's, extract these with a regex.
						var orderedChildNodeIds = serializedChildNodeIds.match(/(\d+)/g);
						$.post('<%= Url.Action("SortPages", "Pages") %>',
							{ parentNodeId : parentNodeId, orderedChildNodeIds : orderedChildNodeIds },
							processJsonMessage,
							"json");
					}
				}
			});
		}
		
		function addDroppable(elementToDrop) {
			$(elementToDrop).droppable({
				accept: "li",
				hoverClass: "droppablepage",
				tolerance: "pointer",
				drop: function(ev, ui) {
					var newParentNodeId = $(this).parents('li').attr('id').substring(5);
					var nodeId = $(ui.draggable).attr('id').substring(5);
					isMoving = true; // disable sorting when dropping on a new parent to prevent the ajax call
					$('#nodeid').val(nodeId);
					$('#newparentnodeid').val(newParentNodeId);
					$('#movedialog').dialog("open");
				}
			});
		}
		
		
	</script>
</asp:Content>
