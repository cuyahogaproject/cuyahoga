<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Articles.ascx.cs" Inherits="Cuyahoga.Modules.Articles.Articles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>


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
					<div class="articlesub"><%= base.GetText("PUBLISHED") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "DateOnline", "{0:D}") %>&nbsp;
						<%= base.GetText("BY") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "CreatedBy.FullName") %>.&nbsp;
						<%= base.GetText("CATEGORY") %>&nbsp;<%# DataBinder.Eval(Container.DataItem, "Category.Title") %>.
					</div>
				</li>
			</itemtemplate>
		</asp:repeater></ul></asp:panel><asp:panel id="pnlArticleDetails" runat="server" visible="False">
	<div class="articlecontent">
		<h4><asp:literal id="litTitle" runat="server"></asp:literal></h4>
		<p><asp:literal id="litContent" runat="server"></asp:literal></p>
		<h5><%= base.GetText("COMMENTS") %></h5>
		<ul class="articlecomments"><asp:repeater id="rptComments" runat="server">
				<itemtemplate>
					<li>
						<p><%# DataBinder.Eval(Container.DataItem, "CommentText") %></p>
						<div class="articlesub">
							<%= base.GetText("BY") %>&nbsp;<asp:placeholder id="plhCommentBy" runat="server"></asp:placeholder>
							- <%# DataBinder.Eval(Container.DataItem, "UpdateTimestamp", "{0:g}") %>
						</div>
					</li>
				</itemtemplate>
			</asp:repeater>
		</ul>
		<asp:panel id="pnlComment" runat="server" visible="False">
			<asp:panel id="pnlAnonymous" runat="server" visible="False"><%= base.GetText("NAME") %>
				<br>
				<asp:textbox id="txtName" runat="server" width="500px" maxlength="100"></asp:textbox>
				<asp:requiredfieldvalidator id="rfvName" cssclass="articleerror" enableclientscript="False" controltovalidate="txtName" display="Dynamic" runat="server"></asp:requiredfieldvalidator>
				<br>
				<%= base.GetText("WEBSITE") %>
				<br>
				<asp:textbox id="txtWebsite" runat="server" width="500px" maxlength="100"></asp:textbox>
			</asp:panel>
			<%= base.GetText("COMMENT") %>
			<br>
			<asp:textbox id="txtComment" runat="server" width="500px" textmode="MultiLine" height="150px"></asp:textbox>
			<asp:requiredfieldvalidator id="rfvComment" cssclass="articleerror" enableclientscript="False" controltovalidate="txtComment" display="Dynamic" runat="server"></asp:requiredfieldvalidator>
			<asp:label id="lblError" runat="server" visible="False" cssclass="articleerror"></asp:label>
			<br>
			<asp:button id="btnSaveComment" runat="server"></asp:button>
		</asp:panel>
		<br>
		<asp:hyperlink id="hplBack" runat="server"></asp:hyperlink>
	</div>
</asp:panel>