<%@ Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<User>" %>
<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
	<title>Cuyahoga Manager :: <%= GlobalResources.EditUserPageTitle %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<h1><%= GlobalResources.EditUserPageTitle %></h1>
	<% using(Html.BeginForm("Update", "Users", new { id = ViewData.Model.Id }, FormMethod.Post, new { id = "userform" })) { %>
		<fieldset>
			<legend><%=GlobalResources.GeneralLabel%></legend>
			<ol>
				<li>
					<label for="UserName"><%=GlobalResources.UsernameLabel%></label>
					<%=ViewData.Model.UserName%>
				</li>
				<% Html.RenderPartial("SharedUserFormElements", ViewData.Model, ViewData); %>
			</ol>
		</fieldset>
		<% Html.RenderPartial("RoleSelector", ViewData.Model, ViewData); %>
		
		<div id="buttonpanel">
		    <input type="submit" class="abtnsave" value="<%= GlobalResources.SaveButtonLabel %>" /> <strong><%= GlobalResources.Or %></strong> <%= Html.ActionLink(GlobalResources.CancelLabel, "Index", "Users", new {@class="abtncancel"})%>
		</div>
		<%= Html.ClientSideValidation(ViewData.Model, "userform")%>
	<% } %>
	<br /><br />
	<% using (Html.BeginForm("ChangePassword", "Users", new { id = ViewData.Model.Id }, FormMethod.Post, new { id = "passwordform" })) { %>
		<fieldset>
			<legend><%=GlobalResources.ChangePasswordLabel%></legend>
			<ol>
				<li>
					<label for="Password"><%=GlobalResources.PasswordLabel%></label>
					<%=Html.Password("Password", String.Empty)%>
				</li>
				<li>
					<label for="PasswordConfirmation"><%=GlobalResources.ConfirmPasswordLabel%></label>
					<%=Html.Password("PasswordConfirmation", String.Empty)%>
				</li>
			</ol>
		</fieldset>
		
		<div id="buttonpanel">
		    <input type="submit" class="abtnchangepassword" value="<%= GlobalResources.ChangePasswordLabel %>" /> <strong><%= GlobalResources.Or %></strong> <%= Html.ActionLink(GlobalResources.CancelLabel, "Index", "Users", new { @class = "abtncancel" })%>
	    </div>
	    
	    <%= Html.ClientSideValidation(ViewData.Model, "passwordform")%>
	<% } %>
</asp:Content>
