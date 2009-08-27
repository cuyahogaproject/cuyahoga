<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<ModuleViewData>>" %>
<%@ Import Namespace="Cuyahoga.Web.Manager.Model.ViewModels"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.ManageModulesPageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h2><%= GlobalResources.ManageModulesPageTitle %></h2>
	<table class="grid" style="width:100%">
		<thead>
			<tr>
				<th><%= GlobalResources.NameLabel %></th>
				<th><%= GlobalResources.AssemblyLabel %></th>
				<th><%= GlobalResources.LoadOnStartupLabel %></th>
				<th><%= GlobalResources.ActivationStatusLabel %></th>
				<th><%= GlobalResources.InstallationStatusLabel %></th>
				<th><%= GlobalResources.ActionsLabel %></th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var module in Model) { %>
				<tr>
					<td><%= module.ModuleName %></td>
					<td><%= module.AssemblyName %></td>
					<td class="center">
						<% using (Html.BeginForm("Activate", "Modules", new { module.ModuleName }, FormMethod.Post)) { %>
							<input type="checkbox" name="autoactivate" value="true" class="autoactivatebutton" <%= module.AutoActivate ? "checked=\"checked\"" : String.Empty %> <%= module.CanInstall || module.CannotDoAnything ? "disabled = \"disabled\"" : String.Empty %> />
							<input type="hidden" name="autoactivate" value="false" />
						<% } %>
					</td>
					<td><%= module.ActivationStatus %></td>
					<td><%= module.InstallationStatus %> (<%= module.ModuleVersion %>)</td>
					<td>
						<% if (module.CanInstall) { %>
							<% using (Html.BeginForm("Install", "Modules", new { module.ModuleName }, FormMethod.Post)) { %>
								<a href="#" class="installlink"><%= GlobalResources.InstallLabel %></a>
							<% } %>
						<% } %>
						<% if (module.CanUpgrade) { %>
							<% using (Html.BeginForm("Upgrade", "Modules", new { module.ModuleName }, FormMethod.Post)) { %>
								<a href="#" class="upgradelink"><%= GlobalResources.UpgradeLabel %></a>
							<% } %>
						<% } %>
						<% if (module.CanUninstall) { %>
							<% using (Html.BeginForm("Uninstall", "Modules", new { module.ModuleName }, FormMethod.Post)) { %>
								<a href="#" class="uninstalllink"><%= GlobalResources.UninstallLabel %></a>
							<% } %>
						<% } %>
					</td>
				</tr>
			<% } %>
		</tbody>
	</table>
	<script type="text/javascript">
		$(document).ready(function() {
			$('.autoactivatebutton').click(function() {
				$(this).parents('form').submit();
			});

			$('.installlink').click(function() {
				if (confirm('<%= GlobalResources.ModuleInstallConfirmation %>')) {
					$(this).parents('form').submit();
				}
				return false;
			});

			$('.upgradelink').click(function() {
				if (confirm('<%= GlobalResources.ModuleUpgradeConfirmation %>')) {
					$(this).parents('form').submit();
				}
				return false;
			});

			$('.uninstalllink').click(function() {
				if (confirm('<%= GlobalResources.ModuleUninstallConfirmation %>')) {
					$(this).parents('form').submit();
				}
				return false;
			});
		})
	</script>
</asp:Content>
