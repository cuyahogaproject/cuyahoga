<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<SearchIndexProperties>" %>
<%@ Import Namespace="Cuyahoga.Core.Service.Search"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.RebuildFullTextIndexPageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= GlobalResources.RebuildFullTextIndexPageTitle %></h1>
	<% using(Html.BeginForm("RebuildIndex", "SearchIndex")) { %>
		<fieldset>
			<legend><%= GlobalResources.CurrentIndexProperties %></legend>
			<ol>
				<li>
					<label><%= GlobalResources.FullTextIndexDirectoryLabel %></label>
					<%= Model.IndexDirectory %>
				</li>
				<li>
					<label><%= GlobalResources.IndexSizeLabel %></label>
					<%= Model.NumberOfDocuments %>
				</li>
				<li>
					<label><%= GlobalResources.LastModifiedLabel %></label>
					<%= Model.LastModified %>
				</li>
			</ol>
		</fieldset>
		
		<input type="submit" value="<%= GlobalResources.RebuildFullTextIndexLabel %>" />
	<% } %>
	<script type="text/javascript">
		$(document).ready(function() {
			$('input:submit').click(function() {
				$('input:submit').attr('disabled', 'disabled');	
			});
		});
	</script>
</asp:Content>
