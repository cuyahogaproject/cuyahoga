<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Node>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= String.Format(GlobalResources.ManageContentPageTitle, Model.Title) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<div id="selectedsection">
	<% if (ViewData.ContainsKey("ActiveSection")) {
		Section activeSection = (Section)ViewData["ActiveSection"];
		Html.RenderPartial("SelectedSection", activeSection, ViewData);
	} %>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= String.Format(GlobalResources.ManageContentPageTitle, Model.Title) %></h1>
	<ul id="contentblocks">
		<% foreach (Section section in Model.Sections) {
			string editPath = String.IsNullOrEmpty(section.ModuleType.EditPath)
	                  	? "#"
	                  	: String.Format("{0}{1}?nodeid={2}&sectionid={3}", ResolveUrl("~/"), section.ModuleType.EditPath,
	                  	                Model.Id, section.Id);
			%>
			<li id="section_<%= section.Id %>">
				<h3><a href="<%= editPath %>"><%= section.Title %> (<%= section.ModuleType.Name %>)</a></h3>
				<div class="contenteditor"></div>
			</li>
		<% } %>
	</ul>
	<script type="text/javascript">
		$(document).ready(function() {
			$('#contentblocks .contenteditor').hide();

			$('#contentblocks a').click(function() {
				selectSection($(this).parents('li'));
				return false;
			});

			if ($('#contentblocks li').length > 0) {
				selectSection($('#contentblocks li:first'));
			}
		})

		function selectSection(sectionElement) {
			$('#contentblocks li').removeClass('active').children('.contenteditor').hide();
			
			var editUrl = sectionElement.find('a:first').attr('href');
			var contentEditorDiv = sectionElement.children('.contenteditor:first');
			if (editUrl != "#" && contentEditorDiv.children().length == 0) {
				contentEditorDiv.append('<iframe src="' + editUrl + '" style="width:100%;height:400px"');			
			}
			contentEditorDiv.slideDown();
			sectionElement.addClass('active');

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
		}
	</script>
	
</asp:Content>
