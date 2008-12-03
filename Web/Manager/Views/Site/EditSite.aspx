<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="EditSite.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Site.EditSite" %>
<%@ Import Namespace="Cuyahoga.Core.Service.Membership"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
	<h2><%= GlobalResources.TasksLabel %></h2>
	<ul>
		<% if (Html.HasRight(User, Rights.CreateSite)) { %>
			<li><%= Html.ActionLink(GlobalResources.CreateSiteLabel, "New")%></li>
		<% } %>
	</ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
</asp:Content>
