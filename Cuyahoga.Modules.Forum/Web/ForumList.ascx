<%@ control language="c#" autoeventwireup="false" codebehind="ForumList.ascx.cs"
	inherits="Cuyahoga.Modules.Forum.ForumList" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ register tagprefix="cc1" namespace="Cuyahoga.ServerControls" assembly="Cuyahoga.ServerControls" %>
<asp:panel id="dummy" runat="server">
	<asp:placeholder id="phForumTop" runat="server"></asp:placeholder>
	<table class="grid" id="tblCategoryTable" cellspacing="0" cellpadding="0">
		<tr class="gridsubheader">
			<td width="1%">
				&nbsp;</td>
			<td align="left" width="58%">
				<asp:label id="lblForum" runat="server" visible="false">Forum</asp:label></td>
			<td align="left" width="8%">
				<asp:label id="lblHdrTopics" runat="server">Topics</asp:label></td>
			<td align="left" width="8%">
				<asp:label id="lblHdrNumPosts" runat="server">NumPosts</asp:label></td>
			<td align="left" width="25%">
				<asp:label id="lblHdrLastPost" runat="server">Lastpost</asp:label></td>
		</tr>
	</table>
	<table class="grid">
		<asp:repeater id="rptCategoryList" runat="server">
			<itemtemplate>
				<tr class="forumrow">
					<td colspan="5" width="100%" class="gridsubheader">
						<asp:hyperlink id="hplForumCategory" runat="server" cssclass="forum">Category</asp:hyperlink></td>
				</tr>
				<asp:repeater id="rptForumList" runat="server">
					<itemtemplate>
						<tr class="forumrow">
							<td width="1%" valign="top" class="newcolum">
								&nbsp;</td>
							<td width="58%" class="forumcolumn">
								<%# GetForumLink(Container.DataItem) %>
								<br>
								<%# DataBinder.Eval(Container.DataItem, "Description") %>
							</td>
							<td width="8%" valign="top" class="forumcolumn">
								<%# DataBinder.Eval(Container.DataItem, "NumTopics") %>
							</td>
							<td width="8%" valign="top" class="forumcolumn">
								<%# DataBinder.Eval(Container.DataItem, "NumPosts") %>
							</td>
							<td width="25%" valign="top" class="lastpostcolumn">
								<%# DataBinder.Eval(Container.DataItem, "LastPosted") %>
							</td>
						</tr>
					</itemtemplate>
					<alternatingitemtemplate>
						<tr class="forumrowalt">
							<td width="1%" valign="top" class="newcolumn">
								&nbsp;</td>
							<td width="58%" class="forumcolumn">
								<%# GetForumLink(Container.DataItem) %>
								<br>
								<%# DataBinder.Eval(Container.DataItem, "Description") %>
							</td>
							<td width="8%" valign="top" class="forumcolumn">
								<%# DataBinder.Eval(Container.DataItem, "NumTopics") %>
							</td>
							<td width="8%" valign="top" class="forumcolumn">
								<%# DataBinder.Eval(Container.DataItem, "NumPosts") %>
							</td>
							<td width="25%" valign="top" class="lastpostcolumn">
								<%# DataBinder.Eval(Container.DataItem, "LastPosted") %>
							</td>
						</tr>
					</alternatingitemtemplate>
				</asp:repeater>
			</itemtemplate>
		</asp:repeater>
	</table>
	<asp:placeholder id="phForumFooter" runat="server"></asp:placeholder>
</asp:panel>
&nbsp; 