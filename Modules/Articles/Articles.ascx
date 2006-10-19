<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Articles.ascx.cs" Inherits="Cuyahoga.Modules.Articles.Articles" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:panel id="pnlArticleList" runat="server" visible="False">
	<ul class="articlelist">
		<asp:repeater id="rptArticles" runat="server" enableviewstate="False">
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
</asp:panel>
<asp:panel id="pnlArticleDetails" runat="server" visible="False">
	<div class="articlecontent">
		<h4><asp:literal id="litTitle" runat="server"></asp:literal></h4>
		<p><asp:literal id="litContent" runat="server"></asp:literal></p>
		<asp:panel id="pnlComments" runat="server">
		<h5 id="comments"><%= base.GetText("COMMENTS") %></h5>
		<ul class="articlecomments">
			<asp:repeater id="rptComments" runat="server">
				<itemtemplate>
					<li>
						<p><%# DataBinder.Eval(Container.DataItem, "CommentText") %></p>
						<div class="articlesub">
							<%= base.GetText("BY") %>
							<asp:placeholder id="plhCommentBy" runat="server"></asp:placeholder>
							-
							<asp:literal id="litUpdateTimestamp" runat="server"></asp:literal>
						</div>
					</li>
				</itemtemplate>
			</asp:repeater>
		</ul>
		</asp:panel>
		<asp:panel id="pnlComment" visible="False" runat="server">
			<asp:panel id="pnlAnonymous" visible="False" runat="server">
				<%= base.GetText("NAME") %>
				<br/>
				<asp:textbox id="txtName" runat="server" maxlength="100" width="500px"></asp:textbox>
				<asp:requiredfieldvalidator id="rfvName" runat="server" display="Dynamic" controltovalidate="txtName" enableclientscript="False"
					cssclass="articleerror"></asp:requiredfieldvalidator>
				<br/>
				<%= base.GetText("WEBSITE") %>
				<br/>
				<asp:textbox id="txtWebsite" runat="server" maxlength="100" width="500px"></asp:textbox>
			</asp:panel>
			<%= base.GetText("COMMENT") %>
			<br/>
			<asp:textbox id="txtComment" runat="server" width="500px" height="150px" textmode="MultiLine"></asp:textbox>
			<asp:requiredfieldvalidator id="rfvComment" runat="server" display="Dynamic" controltovalidate="txtComment"
				enableclientscript="False" cssclass="articleerror"></asp:requiredfieldvalidator>
			<asp:label id="lblError" visible="False" runat="server" cssclass="articleerror"></asp:label>
			<br/>
			<asp:button id="btnSaveComment" runat="server"></asp:button>
		</asp:panel><br/>
		<asp:hyperlink id="hplBack" runat="server"></asp:hyperlink></div>
</asp:panel>
