<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Articles.ascx.cs" Inherits="Cuyahoga.Web.Modules.Articles.Articles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>


<asp:panel id="pnlArticleList" visible="False" runat="server">
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
					<div class="articlesub"><%= base.GetText("PUBLISHED") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "DateOnline", "{0:D}") %>&nbsp;
						<%= base.GetText("BY") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "CreatedBy.UserName") %>.&nbsp;
						<%= base.GetText("CATEGORY") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "Category.Title") %>.
					</div>
				</li>
			</itemtemplate>
		</asp:repeater></ul></asp:panel><asp:panel id="pnlArticleDetails" visible="False" runat="server">
	<div class="articlecontent">
		<h4><asp:literal id="litTitle" runat="server"></asp:literal></h4>
		<p><asp:literal id="litContent" runat="server"></asp:literal></p>
		<h5><%= base.GetText("COMMENTS") %></h5>
		<ul class="articlecomments"><asp:repeater id="rptComments" runat="server">
				<itemtemplate>
					<li>
						<p><%# DataBinder.Eval(Container.DataItem, "CommentText") %></p>
						<div class="articlesub">
							<%= base.GetText("BY") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "User.UserName") %>
							- <%# DataBinder.Eval(Container.DataItem, "UpdateTimestamp", "{0:g}") %>
						</div>
					</li>
				</itemtemplate>
			</asp:repeater></ul><asp:panel id="pnlComment" visible="False" runat="server"><%= base.GetText("COMMENT") %><br><asp:textbox id="txtComment" runat="server" height="150px" textmode="MultiLine" width="500px"></asp:textbox><asp:label id="lblError" visible="False" runat="server" enableviewstate="False" cssclass="articleerror"></asp:label><br><asp:button id="btnSaveComment" runat="server"></asp:button></asp:panel><br><asp:hyperlink id="hplBack" runat="server"></asp:hyperlink></div>
</asp:panel>
