<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Articles.ascx.cs" Inherits="Cuyahoga.Web.Modules.Articles.Articles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:panel id="pnlArticleList" runat="server" visible="False">
	<ul class="articlelist"><asp:repeater id="rptArticles" runat="server" enableviewstate="False">
			<itemtemplate>
				<li>
					<h4><asp:hyperlink id="hplTitle" runat="server"><%# DataBinder.Eval(Container.DataItem, "Title") %></asp:hyperlink></h4>
					<p><%# DataBinder.Eval(Container.DataItem, "Summary") %></p>
					<div class="articlesub">Published <%# DataBinder.Eval(Container.DataItem, "DateOnline", "{0:f}") %>
						by <%# DataBinder.Eval(Container.DataItem, "CreatedBy.UserName") %>.
						Category <%# DataBinder.Eval(Container.DataItem, "Category.Title") %>;
					</div>
				</li>
			</itemtemplate>
		</asp:repeater></ul>
</asp:panel>
<asp:panel id="pnlArticleDetails" runat="server" visible="False">
	<asp:label id="lblTitle" runat="server" font-bold="True"></asp:label>
	<div><asp:literal id="litContent" runat="server"></asp:literal></div>
</asp:panel>
