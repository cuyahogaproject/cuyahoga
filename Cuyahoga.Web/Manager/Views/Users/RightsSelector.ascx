<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Role>" %>
<fieldset style="border:none">
	<li>
	<fieldset>  
		<legend><%= GlobalResources.RightsLabel %></legend>  
		<ol>  
			<% foreach (Right right in (IEnumerable<Right>)ViewData["Rights"]) { %>
			<li>  
				<% if (Html.HasRight(Page.User, right.Name)) {  %>
					<input type="checkbox" name="rightIds" id="Right_<%= right.Id %>" value="<%= right.Id %>" <%= ViewData.Model.HasRight(right.Name) ? "checked" : String.Empty %> />
				<% } else { %>
					<input type="checkbox" name="rightIdsHidden" id="Checkbox1" value="" disabled="disabled" <%= ViewData.Model.HasRight(right.Name) ? "checked" : String.Empty %> />
					<% if (ViewData.Model.HasRight(right.Name)) { %>
						<input type="hidden" name="rightIds" value="<%= right.Id %>" />
					<% } %>
				<% } %>
				<label for="Right_<%= right.Id %>"><%= right.Name %></label>  
			</li>
			<% } %>
		</ol>
	</fieldset>
</li>
</fieldset>