<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Dialog.Master" AutoEventWireup="true" CodeBehind="SectionProperties.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Sections.SectionProperties" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
	<% using (Html.BeginForm("UpdateSection", "Sections", new { id = ViewData.Model.Id }, FormMethod.Post, new { @id = "sectionform" })) { %>
		<% Html.RenderPartial("SharedSectionElements", ViewData.Model); %>
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
	<% } %>
</asp:Content>
