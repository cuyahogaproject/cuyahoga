<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Articles.ascx.cs" Inherits="Cuyahoga.Modules.Articles.Web.Articles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ register assembly="Cuyahoga.ServerControls" namespace="Cuyahoga.ServerControls"	tagprefix="cc1" %>
<asp:panel id="pnlArticleList" runat="server">
	<ul class="articlelist">
		<asp:repeater id="rptArticles" runat="server" enableviewstate="False" onitemdatabound="rptArticles_ItemDataBound">
			<itemtemplate>
				<li>
					<h4>
						<asp:hyperlink id="hplTitle" runat="server">
							<%# DataBinder.Eval(Container.DataItem, "Title") %>
						</asp:hyperlink></h4>
					<asp:panel id="pnlSummary" runat="server" enableviewstate="false" visible="False">
						<%# DataBinder.Eval(Container.DataItem, "Summary") %>
					</asp:panel>
					<asp:panel id="pnlContent" runat="server" enableviewstate="false" visible="False">
							<%# DataBinder.Eval(Container.DataItem, "Content") %>
					</asp:panel>
					<div class="articlesub">
						<%= base.GetText("PUBLISHED") %>
						<asp:literal id="litDateOnline" runat="server"></asp:literal>
						<%= base.GetText("BY") %>
						<asp:hyperlink id="hplAuthor" runat="server"></asp:hyperlink>
						-
						<%= base.GetText("CATEGORY") %>
						<asp:hyperlink id="hplCategory" runat="server"></asp:hyperlink>
						-
						<asp:hyperlink id="hplComments" runat="server"></asp:hyperlink>
					</div>
				</li>
			</itemtemplate>
		</asp:repeater></ul>
		<div class="pager">
			<cc1:pager id="pgrArticles" runat="server" controltopage="rptArticles" hidewhenonepage="true" onpagechanged="pgrArticles_PageChanged" pagerlinkmode="HyperLinkPathInfo" />
		</div>
</asp:panel>