<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Dialog.Master" Inherits="System.Web.Mvc.ViewPage<SectionViewData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.SectionPropertiesPageTitle %></title>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h2><%= String.Format(GlobalResources.SectionPropertiesPageTitle, Model.Section.Title) %></h2>
	<% using (Html.BeginForm("UpdateSection", "Sections", new { Id = Model.Section.Id }, FormMethod.Post, new { @id = "sectionform" })) { %>
		<% Html.RenderPartial("SharedSectionElements", Model.Section); %>
		<div id="buttonpanel">
			<input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" />
		</div>
	<% } %>
</asp:Content>
