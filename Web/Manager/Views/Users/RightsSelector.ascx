<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RightsSelector.ascx.cs" Inherits="Cuyahoga.Web.Manager.Views.Users.RightsSelector" %>
<fieldset style="border:none">
	<li>
	<fieldset>  
		<legend><%= GlobalResources.RightsLabel %></legend>  
		<ol>  
			<% foreach (Right right in (IEnumerable<Right>)ViewData["Rights"]) { %>
			<li>  
				<input type="checkbox" name="rightIds" id="Right_<%= right.Id %>" value="<%= right.Id %>" <%= ViewData.Model.HasRight(right.Name) ? "checked" : String.Empty %> />
				<label for="Right_<%= right.Id %>"><%= right.Name %></label>  
			</li>
			<% } %>
		</ol>
	</fieldset>
</li>
</fieldset>