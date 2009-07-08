<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Views/Shared/ModuleAdmin.Master" Inherits="System.Web.Mvc.ViewPage<StaticHtmlContent>" %>
<%@ Import Namespace="System.Web.Mvc"%>
<%@ Import Namespace="Cuyahoga.Modules.StaticHtml"%>
<%@ Import Namespace="System.Web.Mvc.Html"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title><%= GlobalResources.EditContentPageTitle %></title>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<h2><%= GlobalResources.EditContentPageTitle %></h2>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% using (Html.BeginForm("SaveContent", "ManageContent", new { NodeId = Request.Params["nodeId"], SectionId = Request.Params["SectionId"] }, FormMethod.Post)) { %>
		<%= Html.TextArea("content", Model.Content, new { style = "width:100%;height:300px" }) %>
		<input type="submit" value="Save" />
	<% } %>
</asp:Content>
