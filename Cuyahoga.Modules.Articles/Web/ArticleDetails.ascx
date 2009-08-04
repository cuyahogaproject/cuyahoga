<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticleDetails.ascx.cs" Inherits="Cuyahoga.Modules.Articles.Web.ArticleDetails" %>
<%@ register assembly="Cuyahoga.ServerControls" namespace="Cuyahoga.ServerControls"	tagprefix="cc1" %>
<asp:panel id="pnlArticleDetails" runat="server" visible="False">
	<div class="articlecontent">
		<h4><asp:literal id="litTitle" runat="server"></asp:literal></h4>
		<p><asp:literal id="litContent" runat="server"></asp:literal></p>
		<asp:panel id="pnlArticleInfo" cssclass="articlesub" visible="False" runat="server">
			<asp:label id="lblDateOnline" runat="server"></asp:label>
			<asp:literal id="litAuthor" runat="server"></asp:literal>
			<asp:hyperlink id="hplAuthor" runat="server"></asp:hyperlink>
			<asp:literal id="litCategory" runat="server"></asp:literal>
			<cc1:categorydisplay id="cadCategories" runat="server" />
			<asp:hyperlink id="hplComments" runat="server"></asp:hyperlink>
		</asp:panel>
		<br />
		<asp:panel id="pnlComments" runat="server">
		<h5 id="comments"><%= base.GetText("COMMENTS") %></h5>
		<ul class="articlecomments">
			<asp:repeater id="rptComments" runat="server">
				<itemtemplate>
					<li>
						<p><%# Server.HtmlEncode(Eval("CommentText").ToString()) %></p>
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
			<asp:button id="btnSaveComment" runat="server" onclick="btnSaveComment_Click"></asp:button>
		</asp:panel><br/>
		<asp:hyperlink id="hplBack" runat="server"></asp:hyperlink></div>
</asp:panel>
