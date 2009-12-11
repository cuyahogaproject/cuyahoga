<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ModuleAdminViewModel<FileResource>>" %>
<%@ Import Namespace="Cuyahoga.Web.Mvc.UI"%>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<fieldset>
	<legend><%=GlobalResources.FilePropertiesLabel%></legend>
	<ol>
		<li>
			<label for="FileData"><%= GlobalResources.FileLabel %></label>
			<input type="file" id="FileData" name="filedata" />
			<%= Html.Hidden("FileName", Model.ModuleData.FileName) %>
			<%= Model.ModuleData.FileName %>
		</li>
		<li>
			<label for="Title"><%=GlobalResources.TitleLabel%></label>
			<%=Html.TextBox("Title", Model.ModuleData.Title, new {style = "width:435px"})%>
		</li>
		<li>
			<label for="Summary"><%=GlobalResources.SummaryLabel%></label>
			<%=Html.TextArea("Summary", Model.ModuleData.Summary, new {style = "width:435px"})%>
		</li>
	</ol>
</fieldset>
<fieldset>
	<legend><%=GlobalResources.PublishingLabel%></legend>
	<ol>
		<li>
			<label for="Syndicate"><%=GlobalResources.SyndicateLabel%></label>
			<%=Html.CheckBox("Syndicate", Model.ModuleData.Syndicate)%>			
		</li>
		<li>
			<label for="PublishedAt"><%=GlobalResources.PublishedLabel%></label>
			<%=Html.DateTimeInput("PublishedAt", Model.ModuleData.PublishedAt)%>
		</li>
		<li>
			<label for="PublishedUntil"><%=GlobalResources.PublishedUntilLabel%></label>
			<%=Html.DateTimeInput("PublishedUntil", Model.ModuleData.PublishedUntil)%>
		</li>
	</ol>
</fieldset>
<fieldset>
	<legend><%=GlobalResources.PermissionsLabel%></legend>
	<ol>
		<li>
			<fieldset>  
				<legend><%= GlobalResources.RolesViewAllowedLabel %></legend>  
				<ol>  
					<% foreach (Role role in (IEnumerable<Role>)ViewData["Roles"]) { %>
					<li>  
						<input type="checkbox" name="RoleIds" id="Role_<%= role.Id %>" value="<%= role.Id %>" <%= Model.ModuleData.ContentItemPermissions.Any(cip => cip.ViewAllowed && cip.Role.Id == role.Id) ? "checked" : String.Empty %> />
						<label for="Role_<%= role.Id %>"><%= role.Name %></label>  
					</li>
					<% } %>
				</ol>
			</fieldset>
		</li>
	</ol>
</fieldset>