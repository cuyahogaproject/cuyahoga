<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Dialog.Master" AutoEventWireup="true" CodeBehind="NewSectionDialog.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Sections.NewSectionDialog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.core.js") %>"></script>
	<script type="text/javascript" src="<%= Url.Content("~/manager/Scripts/ui.dialog.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
<% using (Html.BeginForm("AddSectionToPage", "Sections", FormMethod.Post, new { @id = "newsectionform" })) { %>
	<%= Html.Hidden("NodeId", ViewData["NodeId"]) %>
	<%= Html.Hidden("ModuleTypeId", ViewData.Model.ModuleType.ModuleTypeId) %>
	<%= Html.Hidden("section.PlaceHolderId", ViewData.Model.PlaceholderId) %>
	<% Html.RenderPartial("SharedSectionElements", ViewData.Model); %>
<% } %>
<script type="text/javascript">
	$(document).ready(function() {
		// Hack to close the dialog with 
		<% if (ViewData.ContainsKey("CanCloseDialog")) { %>
			if (window.parent.$('#newsectiondialog').length > 0) {
				window.parent.$('#newsectiondialog').dialog('close'); 
			}
		<% } %>
	})
</script>
</asp:Content>
