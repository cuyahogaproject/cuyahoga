<%@ register tagprefix="cc1" namespace="Cuyahoga.ServerControls" assembly="Cuyahoga.ServerControls" %>
<%@ control language="c#" autoeventwireup="false" codebehind="ForumViewPost.ascx.cs"
	inherits="Cuyahoga.Modules.Forum.ForumViewPost" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:placeholder id="phForumTop" runat="server"></asp:placeholder>
<table class="grid" width="100%">
	<tbody>
		<tr class="forumrow">
			<td class="gridsubheader">
				<asp:label id="lblTopic" runat="server">Forum title / info</asp:label></td>
			<td class="gridsubheader" align="right">
                <asp:LinkButton ID="lbtnRemove" runat="server" OnClick="lbtnRemove_Click" cssclass="forum" Visible=false>Remove post</asp:LinkButton>
                &nbsp;
				<asp:hyperlink id="hplReply" runat="server" cssclass="forum">Add reply</asp:hyperlink>&nbsp;
				<asp:hyperlink id="hplNewTopic" runat="server" cssclass="forum">New Topic</asp:hyperlink></td>
		</tr>
		<tr>
			<td colspan="2">
				<table id="tblForumPostOrigTable" cellspacing="1" cellpadding="1" width="100%" border="0">
					<tr class="forumrowalt">
						<td width="20%" style="height: 46px">
							<asp:hyperlink id="hplAuthor" runat="server">Label</asp:hyperlink></td>
						<td width="80%" style="height: 46px">
							<table width="100%">
								<tr>
									<td width="80%">
										<asp:literal id="lblPostedDate" runat="server">Label</asp:literal></td>
									<td width="20%">
										<asp:hyperlink id="hplQuotePost" runat="server" cssclass="forum" visible="False">Quote</asp:hyperlink></td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr class="forumrow">
			<td width="20%" style="height: 21px">
				<asp:label id="lblUserInfo" runat="server">Label</asp:label></td>
			<td width="80%" style="height: 21px">
				<asp:literal id="lblMessages" runat="server">Label</asp:literal></td>
		</tr>
		<asp:panel id="pnlAttachment" runat="server" visible="False">
			<tr class="forumrow">
				<td width="20%">
					<asp:label id="Label1" runat="server">&nbsp;</asp:label></td>
				<td width="80%">
					<asp:label id="lblAttachment" runat="server">Label</asp:label>&nbsp;
					<asp:hyperlink id="hplPostAttachment" runat="server" cssclass="forum">HyperLink</asp:hyperlink>&nbsp;
					<asp:literal id="ltlFileinfo" runat="server"></asp:literal></td>
			</tr>
		</asp:panel>
		<tr class="forumrowalt">
			<td width="20%" style="height: 21px">
				&nbsp;</td>
			<td width="80%" style="height: 21px">
				&nbsp;</td>
		</tr>
		<tr>
			<td width="20%" bgcolor="darkgray" height="10">
			</td>
			<td width="80%" bgcolor="darkgray" height="10">
			</td>
		</tr>
		<asp:repeater id="rptForumPostRepliesList" runat="server">
			<itemtemplate>
				<tr class="forumrowalt">
					<td width="20%" valign="top">
						<%# GetUserProfileLink(Container.DataItem) %>
					</td>
					<td width="80%" valign="top">
						<table width="100%">
							<tr class="forumrowalt">
								<td width="80%">
									<%# GetPostedDate(Container.DataItem) %>
								</td>
								<td width="20%">
                                        <asp:LinkButton ID="lbtnRemove" runat="server" OnClick="lbtnRemove_Click" cssclass="forum" CommandArgument="<%# GetForumPostId(Container.DataItem) %>" Visible=false>Remove post</asp:LinkButton>
                                        &nbsp;
                						<%# GetQuoteLink(Container.DataItem) %>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr class="forumrow">
					<td width="20%">
						&nbsp;</td>
					<td width="80%">
						<%# GetMessage(Container.DataItem) %>
					</td>
				</tr>
				<asp:panel id="pnlReplyAttachment" runat="server" visible="False">
					<tr class="forumrow">
						<td width="20%">
							<asp:label id="Label2" runat="server">&nbsp;</asp:label></td>
						<td width="80%">
							<asp:label id="lblReplyAttachment" runat="server">Label</asp:label>&nbsp;
							<asp:hyperlink id="hplReplyttachment" runat="server" cssclass="forum">HyperLink</asp:hyperlink>&nbsp;
							<asp:literal id="ltlReplyFileinfo" runat="server"></asp:literal></td>
					</tr>
				</asp:panel>
				<tr class="forumrowalt">
					<td width="20%">
						&nbsp;</td>
					<td width="80%">
						&nbsp;</td>
				</tr>
				<tr>
					<td width="20%" bgcolor="darkgray" height="10">
					</td>
					<td width="80%" bgcolor="darkgray" height="10">
					</td>
				</tr>
			</itemtemplate>
		</asp:repeater>
</TBODY></TABLE><asp:placeholder id="phForumFooter" runat="server"></asp:placeholder>
