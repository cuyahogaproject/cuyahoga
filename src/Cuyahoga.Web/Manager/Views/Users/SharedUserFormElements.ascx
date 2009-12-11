<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<User>" %>
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