<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SharedSectionViewData>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.ManageSharedContentPageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TasksLabel %></h2>
	<%= Html.ActionLink(GlobalResources.CreateSharedSectionLabel, "NewShared", "Sections", null, new { @class = "createlink" }) %>
	<div id="selectedsection">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h2><%= GlobalResources.ManageSharedContentPageTitle %></h2>
		<table class="grid" style="width:100%">
		<thead>
			<tr>
				<th><%= GlobalResources.SectionNameLabel %></th>
				<th><%= GlobalResources.ModuleTypeLabel %></th>
				<th><%= GlobalResources.AttachedToLabel %></th>
				<th><%= GlobalResources.ActionsLabel %></th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var data in Model) { %>
				<tr id="section-<%= data.Section.Id %>" class="sectionrow">
					<td><%= data.Section.Title %></td>
					<td><%= data.Section.ModuleType.Name %></td>
					<td><%= data.AttachedToTemplates %></td>
					<td>
						<%= Html.ActionLink(GlobalResources.AttachToTemplateLabel, "AttachSectionToTemplate", "Sections", new { id = data.Section.Id }, null) %>
						<%--<%= Html.ActionLink(GlobalResources.MoveSectionToPageLabel, "MoveSectionToPage", "Sections", new { id = section.Id }, null) %>--%>
						<% using (Html.BeginForm("DeleteShared", "Sections", new { id = data.Section.Id }, FormMethod.Post)) { %>
							<a href="#" class="abtndelete"><%= GlobalResources.DeleteButtonLabel %></a>
						<% } %>
					</td>
				</tr>		
			<% } %>
		</tbody>
	</table>
	<script type="text/javascript">
		$(document).ready(function() {
			$('.sectionrow').click(function() {
				selectSection($(this));
			})
			
			<% if (ViewData.ContainsKey("ActiveSection")) {
				var activeSection = (Section)ViewData["ActiveSection"]; %>
				selectSection($('#section-<%= activeSection.Id %>'));
			<% } %>
		})

		function selectSection(sectionElement) {
			$('.sectionrow').removeClass('selected');
			sectionElement.addClass('selected');
			
			// Show section properties for the selected section.
			selectedSectionId = sectionElement.attr('id').substring(8); // strip 'section-'
			$('#selectedsection').unbind();
			$('#selectedsection').load('<%= Url.Action("SelectSection", "Sections") %>', { sectionid: selectedSectionId }, ajaxifySelectedSectionForms);
		}

		function ajaxifySelectedSectionForms() {
			$('#selectedsection form').ajaxForm({
				target: '#selectedsection',
				success: function() {
					movePartialMessages();
					ajaxifySelectedSectionForms();
				}
			});
			
			// Add handler for connection action
			$('#actionname').change(function() {
				$('#connecttoid').html('');
				$.getJSON('<%= Url.Action("GetAvailableConnectionsForSectionAndAction", "Sections") %>', { sectionid:selectedSectionId, actionname: $('#actionname').val() }, function(data) {
					$.each(data, function(i, item) {
						$('#connecttoid').append('<option value="' + item.SectionId + '">' + item.PageName + ' - ' + item.SectionName + '</option>');
					});
				});
			})
		}
	</script>
</asp:Content>
