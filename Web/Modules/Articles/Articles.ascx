<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Articles.ascx.cs" Inherits="Cuyahoga.Web.Modules.Articles.Articles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<asp:panel id="pnlArticleList" runat="server" visible="False">
	<ul class="articlelist"><asp:repeater id="rptArticles" runat="server" enableviewstate="False">
			<itemtemplate>
				<li>
					<h4><asp:hyperlink id="hplTitle" runat="server"><%# DataBinder.Eval(Container.DataItem, "Title") %></asp:hyperlink></h4>
					<asp:panel id="pnlSummary" runat="server" enableviewstate="false" visible="False">
						<%# DataBinder.Eval(Container.DataItem, "Summary") %>
					</asp:panel>
					<asp:panel id="pnlContent" runat="server" enableviewstate="false" visible="False">
						<p>
						<%# DataBinder.Eval(Container.DataItem, "Content") %>
						</p>
					</asp:panel>
					<div class="articlesub"><%= base.GetText("PUBLISHED") %> <%# DataBinder.Eval(Container.DataItem, "DateOnline", "{0:D}") %>
						<%= base.GetText("BY") %> <%# DataBinder.Eval(Container.DataItem, "CreatedBy.UserName") %>.
						<%= base.GetText("CATEGORY") %> <%# DataBinder.Eval(Container.DataItem, "Category.Title") %>.
					</div>
				</li>
			</itemtemplate>
		</asp:repeater></ul>
</asp:panel>
<asp:panel id="pnlArticleDetails" runat="server" visible="False">
	<div class="articlecontent">
		<h4><asp:literal id="litTitle" runat="server"></asp:literal></h4>
		<p><asp:literal id="litContent" runat="server"></asp:literal></p>
		<br><asp:hyperlink id="hplBack" runat="server"></asp:hyperlink>
	</div>

</asp:panel>
