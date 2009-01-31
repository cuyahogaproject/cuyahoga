<%@ Page Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="ViewPage<Site>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<fieldset>
		<ol>
			<li>
				<label><%= GlobalResources.SiteLabel %></label>
				<%= ViewData.Model.Name %>
			</li>
			<li>
				<label><%= GlobalResources.SiteUrlLabel %></label>
				<%= ViewData.Model.SiteUrl %>
			</li>
		</ol>
	</fieldset>
	<p>
		<%= Html.ActionLink(GlobalResources.JumpToSiteAdminLabel, "SetSite", "Dashboard", new RouteValueDictionary { {"siteId", ViewData.Model.Id} }, null )%><br />
		<%= GlobalResources.NewSiteWarningText %>
	</p>
</asp:Content>
