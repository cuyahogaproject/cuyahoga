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
						<p><%# DataBinder.Eval(Container.DataItem, "Summary") %></p>
						<asp:hyperlink id="hplReadMore" runat="server" />
					</asp:panel>
					<asp:panel id="pnlContent" runat="server" enableviewstate="false" visible="False">
							<%# DataBinder.Eval(Container.DataItem, "Content") %>
					</asp:panel>
					<asp:panel id="pnlArticleInfo" cssclass="articlesub" visible="False" runat="server">
						<asp:label id="lblDateOnline" runat="server"></asp:label>
						<asp:literal id="litAuthor" runat="server"></asp:literal>
						<asp:hyperlink id="hplAuthor" runat="server"></asp:hyperlink>
						<asp:literal id="litCategory" runat="server"></asp:literal>
						<cc1:categorydisplay id="cadCategories" runat="server" />
						<asp:hyperlink id="hplComments" runat="server"></asp:hyperlink>
					</asp:panel>
				</li>
			</itemtemplate>
		</asp:repeater></ul>
		<div class="pager">
			<cc1:pager id="pgrArticles" runat="server" controltopage="rptArticles" hidewhenonepage="true" onpagechanged="pgrArticles_PageChanged" pagerlinkmode="HyperLinkPathInfo" />
		</div>
		<asp:hyperlink id="hplToggleArchive" runat="server" visible="false" />
</asp:panel>