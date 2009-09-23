<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Category>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.EditCategoryPageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= GlobalResources.EditCategoryPageTitle %></h1>
	<% using(Html.BeginForm("Update", "Categories", FormMethod.Post, new { id = "categoryform" })) { %>
		<%= Html.Hidden("id", Model.Id) %>
		<fieldset>
			<legend><%=GlobalResources.GeneralLabel%></legend>
			<ol>
				<% Html.RenderPartial("SharedCategoryElements", Model, ViewData); %>
			</ol>
		</fieldset>
		
		<div id="buttonpanel">
		    <input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" /> <strong><%= GlobalResources.Or %></strong> <%= Html.ActionLink(GlobalResources.CancelLabel, "Index", "Categories", new { @class = "abtncancel" })%>
		</div>
		<%= Html.ClientSideValidation(ViewData.Model, "categoryform") %>		
	<% } %>
</asp:Content>
