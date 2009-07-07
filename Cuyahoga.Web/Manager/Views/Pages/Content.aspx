<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Node>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= String.Format(GlobalResources.ManageContentPageTitle, Model.Title) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= String.Format(GlobalResources.ManageContentPageTitle, Model.Title) %></h1>
	<ul id="contentblocks">
		<% foreach (Section section in Model.Sections) { %>
			<li id="section_<%= section.Id %>">
				<h3><a href="<%="{0}{1}?nodeid={2}&sectionid={3}", ResolveUrl("~/"), section.ModuleType.EditPath, Model.Id, section.Id%>"><%= section.Title %> (<%= section.ModuleType.Name %>)</a></h3>
				<iframe style="width:100%;height:400px"></iframe>
			</li>
		<% } %>
	</ul>
	<script type="text/javascript">
		$(document).ready(function() {
			$('#contentblocks iframe').hide();

			$('#contentblocks a').click(function() {
				selectSection($(this).parents('li'));
				return false;
			});
		})

		function selectSection(sectionElement) {
			$('#contentblocks iframe').hide();
			var editUrl = $(sectionElement).find('a:first').attr('href');
			$(sectionElement).find('iframe').attr('src', editUrl).slideDown();
		}
	</script>
	
</asp:Content>
