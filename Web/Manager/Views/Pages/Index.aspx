<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Pages.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<table class="grid" style="width:100%">
		<thead>
			<tr>
				<th>Page title</th>
				<th>Page url</th>
				<th style="width:120px">Template</th>
				<th style="width:60px">Culture</th>
				<th style="width:120px">Last modified</th>
				<th>&nbsp;</th>
			</tr>
		</thead>
		<tbody>
		<% Html.RenderPartial("PageListItems", ViewData.Model, ViewData); %>
		</tbody>
	</table>
	<script type="text/javascript">
		$(document).ready(function() {
			$('span.children-visible > .expander').toggle(function() {
				toggleHide(this);
			}, function() {
				toggleShow(this);
			});
			
			$('span.children-hidden > .expander').toggle(function() { 
				toggleShow(this);	
			}, function() {
				toggleHide(this);
			});
		})
		
		function toggleHide(expander) {
			$(expander).attr('src', '<%= Url.Content("~/manager/Content/Images/expand.png") %>');
			$(expander).parent().removeClass('children-visible').addClass('children-hidden');
			var nodeId = $(expander).parents('tr').attr('id').substring(5);
			hidePages(nodeId);
		}
		
		function toggleShow(expander) {
			$(expander).attr('src', '<%= Url.Content("~/manager/Content/Images/collapse.png") %>');
			$(expander).parent().removeClass('children-hidden').addClass('children-visible');
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
					// Add toggle handlers to newly added items
					$('.parent-' + parentNodeId + ' .expander').toggle(function() { 
						toggleShow(this);	
					}, function() {
						toggleHide(this);
					});
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

		
	</script>
</asp:Content>
