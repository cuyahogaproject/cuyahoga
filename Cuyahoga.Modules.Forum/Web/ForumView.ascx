<%@ register tagprefix="cc1" namespace="Cuyahoga.ServerControls" assembly="Cuyahoga.ServerControls" %>
<%@ control language="c#" autoeventwireup="false" codebehind="ForumView.ascx.cs"
	inherits="Cuyahoga.Modules.Forum.ForumView" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:placeholder id="phForumTop" runat="server"></asp:placeholder>
<asp:panel runat="server" id="Panel1">
	<table class="grid" width="100%">
		<tr class="gridsubheader">
			<td colspan="5">
				<asp:label id="lblForumName" runat="server">Forum name</asp:label></td>
			<td align="right">
				<asp:hyperlink id="hplNewTopic" runat="server" cssclass="forum">New Topic</asp:hyperlink></td>
		</tr>
	</table>
	<table class="grid">
		<asp:repeater id="rptForumPostList" runat="server">
			<headertemplate>
				<tr class="gridsubheader">
					<td width="45%" align="left">
						<asp:label id="lblHdrTopicTitle" runat="server">Topics</asp:label></td>
					<td align="left" width="13%">
						<asp:label id="lblHdrTopicStarter" runat="server">Topic starter</asp:label></td>
					<td align="left" width="8%">
						<asp:label id="lblHdrReplies" runat="server">Replies</asp:label></td>
					<td align="left" width="8%">
						<asp:label id="lblHdrViews" runat="server">Views</asp:label></td>
					<td align="left" width="25%">
						<asp:label id="lblHdrLastPost" runat="server">Lastpost</asp:label></td>
				</tr>
			</headertemplate>
			<itemtemplate>
				<tr class="forumrow">
					<td width="45%" class="forumcolumn">
						<%# GetTopicLink(Container.DataItem) %>
					</td>
					<td width="13%" valign="top" class="forumcolumn">
						<%# DataBinder.Eval(Container.DataItem, "UserName") %>
					</td>
					<td width="8%" valign="top" class="forumcolumn">
						<%# DataBinder.Eval(Container.DataItem,"Replies") %>
					</td>
					<td width="8%" valign="top" class="forumcolumn">
						<%# DataBinder.Eval(Container.DataItem,"Views") %>
					</td>
					<td width="25%" valign="top" class="forumcolumn">
						<%# DataBinder.Eval(Container.DataItem, "DateCreated") %>
					</td>
				</tr>
			</itemtemplate>
			<alternatingitemtemplate>
				<tr class="forumrowalt">
					<td width="45%" class="forumcolumn">
						<%# GetTopicLink(Container.DataItem) %>
					</td>
					<td width="13%" valign="top" class="forumcolumn">
						<%# DataBinder.Eval(Container.DataItem, "Username") %>
					</td>
					<td width="8%" valign="top" class="forumcolumn">
						<%# DataBinder.Eval(Container.DataItem,"Replies") %>
					</td>
					<td width="8%" valign="top" class="forumcolumn">
						<%# DataBinder.Eval(Container.DataItem,"Views") %>
					</td>
					<td width="25%" valign="top" class="forumcolumn">
						<%# DataBinder.Eval(Container.DataItem, "DateCreated") %>
					</td>
				</tr>
			</alternatingitemtemplate>
		</asp:repeater>
	</table>
</asp:panel>
<p>
</p>
<asp:placeholder id="phForumFooter" runat="server"></asp:placeholder>
