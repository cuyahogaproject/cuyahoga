<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Section>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.NewSharedSectionPageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h2><%= GlobalResources.NewSharedSectionPageTitle %></h2>
	<% using (Html.BeginForm("CreateSharedSection", "Sections", FormMethod.Post, new { @id = "newsectionform" })) { %>
		
		<label for="ModuleTypeId"><%= GlobalResources.SelectModuleTypeLabel %></label>
		<%= Html.DropDownList("ModuleTypeId", ViewData["ModuleTypes"] as SelectList) %>
		
		<div id="sectiondetails">
			<% Html.RenderPartial("SharedSectionElements", Model); %>
		</div>
		<div id="buttonpanel">
		    <input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" /> <%= GlobalResources.Or %> <%= Html.ActionLink(GlobalResources.CancelLabel, "Index", "Sections", null, new { @class = "abtncancel" })%>
		</div>
	<% } %>
	<script type="text/javascript">
		$(document).ready(function() {
			$('#ModuleTypeId').change(function() {
				$('#sectiondetails').load('<%= Url.Action("GetSectionElements", "Sections") %>', { moduletypeid: $('#ModuleTypeId').val() });
			});
		})
	</script>
</asp:Content>
