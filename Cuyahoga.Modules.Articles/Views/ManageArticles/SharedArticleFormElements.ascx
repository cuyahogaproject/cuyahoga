<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ModuleAdminViewModel<Article>>" %>
<%@ Import Namespace="Cuyahoga.Web.Mvc.HtmlEditor"%>
<%@ Import Namespace="Cuyahoga.Web.Mvc.UI"%>
<fieldset>
	<legend><%=GlobalResources.ArticleContentLabel%></legend>
	<ol>
		<li>
			<label for="Title"><%=GlobalResources.TitleLabel%></label>
			<%=Html.TextBox("Title", Model.ModuleData.Title, new {style = "width:435px"})%>
		</li>
		<li>
			<label for="Summary"><%=GlobalResources.SummaryLabel%></label>
			<%=Html.TextArea("Summary", Model.ModuleData.Summary, new {style = "width:435px"})%>
		</li>
		<li>
			<label for="Content"><%=GlobalResources.ContentLabel%></label><br />
			<% 
			string contentCss = Model.Node != null && Model.Node.Template != null
				? Url.Content(Model.CuyahogaContext.CurrentSite.SiteDataDirectory) + Model.Node.Template.EditorCss
				: null;
			%>
			<%= Html.HtmlEditor("Content", Model.ModuleData.Content, contentCss, new { style = "width:100%;height:300px"}) %>
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
