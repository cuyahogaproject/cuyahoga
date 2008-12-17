<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.Index" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.core.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/jquery.scrollfollow.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
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
	<table id="pagegrid" class="grid" style="width:100%">
		<thead>
			<tr>
				<th>Page title</th>
				<th>Page url</th>
				<th style="width:120px">Template</th>
				<th style="width:60px">Culture</th>
				<th style="width:120px">Last modified</th>
			</tr>
		</thead>
		<tbody>
		<% Html.RenderPartial("PageListItems", ViewData.Model, ViewData); %>
		</tbody>
	</table>
	<script type="text/javascript"> 
		var selectedPageRow;
		
		$(document).ready(function() {
			
			$('#taskarea').scrollFollow({
				container : 'contentarea'
			});
			
			$('#pagegrid').click($.delegate({
				'.children-visible' : function(e) { 
					toggleHide(e.target); 
				},
				'.children-hidden' : function(e) { 
					toggleShow(e.target);
				},
				'td' : function(e) {
					selectPage(e.target);
				},
				'span' : function(e) {
					selectPage(e.target);
				}
			}))
			
			selectedPageRow = $('#pagegrid tr.selected');
			
		})	
				
		function toggleHide(expander) {
			$(expander).attr('src', '<%= Url.Content("~/manager/Content/Images/expand.png") %>');
			$(expander).removeClass('children-visible').addClass('children-hidden');
			var nodeId = $(expander).parents('tr').attr('id').substring(5);
			hidePages(nodeId);
		}
		
		function toggleShow(expander) {
			$(expander).attr('src', '<%= Url.Content("~/manager/Content/Images/collapse.png") %>');
			$(expander).removeClass('children-hidden').addClass('children-visible');
			var nodeId = $(expander).parents('tr').attr('id').substring(5);
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
					$('#page-' + parentNodeId).after(data);
				})
			}
			else {
				$('.parent-' + parentNodeId).show();
				// only recurse pages that have their children visible
				$('.parent-' + parentNodeId + ':has(span.children-visible)').each(function(i) {
					showPages($(this).attr('id').substring(5));
				});
			}
		}
		
		function selectPage(pageCell) {
			if (selectedPageRow) {
				selectedPageRow.removeClass('selected');
			}
			selectedPageRow = $(pageCell).parents('tr');
			var nodeId = selectedPageRow.attr('id').substring(5);
			$('#selectedpage').load('<%= Url.Action("SelectPage", "Pages") %>', { 'nodeid' : nodeId });
			selectedPageRow.addClass('selected');
			
		}
		
	</script>
</asp:Content>
