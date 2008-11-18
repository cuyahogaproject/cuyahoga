<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Users.Edit" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<% using(Html.BeginForm("Save", "Users", FormMethod.Post, new { id = "userform" })) { %>
		<fieldset>
			<legend><%=GlobalResources.GeneralLabel%></legend>
			<ol>
				<li>
					<label for="UserName"><%=GlobalResources.UsernameLabel%></label>
					<% if (ViewData.Model.Id > 0) {%>
						<%=ViewData.Model.UserName%>
					<% } else { %>
						<%=Html.TextBox("UserName", ViewData.Model.UserName)%>
					<% } %>
				</li>
				<li>
					<label for="FirstName"><%=GlobalResources.FirstNameLabel%></label>
					<%=Html.TextBox("FirstName", ViewData.Model.FirstName, new {style = "width:300px"})%>
				</li>
				<li>
					<label for="LastName"><%=GlobalResources.LastNameLabel%></label>
					<%=Html.TextBox("LastName", ViewData.Model.LastName, new {style = "width:300px"})%>
				</li>
				<li>
					<label for="Email"><%=GlobalResources.EmailLabel%></label>
					<%=Html.TextBox("Email", ViewData.Model.Email, new {style = "width:300px"})%>
				</li>
				<li>
					<label for="Website"><%=GlobalResources.WebsiteLabel%></label>
					<%=Html.TextBox("Website", ViewData.Model.Website, new {style = "width:300px"})%>
				</li>
				<li>
					<label for="IsActive"><%=GlobalResources.IsActiveLabel%></label>
					<%=Html.CheckBox("IsActive", ViewData.Model.IsActive)%>
				</li>
				<li>
					<label for="TimeZone"><%=GlobalResources.TimeZoneLabel%></label>
					<%=Html.DropDownList("TimeZone", ViewData["TimeZones"] as SelectList)%>
				</li>
				<li>
					<label for="Password"><%=GlobalResources.PasswordLabel%></label>
					<%=Html.Password("Password")%>
				</li>
				<li>
					<label for="ConfirmPassword"><%=GlobalResources.ConfirmPasswordLabel%></label>
					<%=Html.Password("ConfirmPassword")%>
				</li>
			</ol>
		</fieldset>
		<fieldset>
			<li>
			<fieldset>  
				<legend><%= GlobalResources.RolesLabel %></legend>  
				<ol>  
					<% foreach (Role role in (IEnumerable<Role>)ViewData["Roles"]) { %>
					<li>  
						<input type="checkbox" name="RoleIds" id="Role_<%= role.Id %>" value="<%= role.Id %>" <%= ViewData.Model.IsInRole(role) ? "checked" : String.Empty %> />
						<label for="Role_<%= role.Id %>"><%= role.Name %></label>  
					</li>
					<% } %>
				</ol>
			</fieldset>
		</li>
		</fieldset>
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
		<%= GlobalResources.Or %>
		<%= Html.ActionLink(GlobalResources.CancelLabel, "Index", "Users") %>
	<% } %>
</asp:Content>
