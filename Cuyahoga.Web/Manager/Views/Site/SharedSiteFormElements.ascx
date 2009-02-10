<%@ Import Namespace="Cuyahoga.Core.Domain"%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site>" %>
<fieldset>
	<legend><%=GlobalResources.GeneralLabel%></legend>
	<ol>
		<li>
			<label for="Name"><%=GlobalResources.NameLabel%></label>
			<%=Html.TextBox("Name", ViewData.Model.Name, new { style = "width:300px" })%>
		</li>				
		<li>
			<label for="SiteUrl"><%=GlobalResources.SiteUrlLabel%></label>
			<%=Html.TextBox("SiteUrl", ViewData.Model.SiteUrl, new { style = "width:300px" })%>
		</li>
		<li>
			<label for="WebmasterEmail"><%=GlobalResources.WebmasterEmailLabel%></label>
			<%=Html.TextBox("WebmasterEmail", ViewData.Model.WebmasterEmail, new { style = "width:300px" })%>
		</li>
		<li>
			<label for="UseFriendlyUrls"><%=GlobalResources.UseFriendlyUrlsLabel%></label>
			<%= Html.CheckBox("UseFriendlyUrls", ViewData.Model.UseFriendlyUrls)%>
		</li>
	</ol>
</fieldset>