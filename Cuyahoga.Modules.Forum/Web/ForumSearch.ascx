<%@ register tagprefix="cc1" namespace="Cuyahoga.ServerControls" assembly="Cuyahoga.ServerControls" %>
<%@ control language="c#" autoeventwireup="false" codebehind="ForumSearch.ascx.cs"
	inherits="Cuyahoga.Modules.Forum.ForumSearch" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:placeholder id="phForumTop" runat="server"></asp:placeholder>
<asp:panel id="pnlSearch" runat="server">
	<table class="grid" id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
		<tr class="gridsubheader">
			<td>
				<asp:label id="lblSearch" runat="server">Label</asp:label></td>
		</tr>
		<tr>
			<td align="center">
				&nbsp;
			</td>
		</tr>
		<tr>
			<td>
				<asp:textbox id="txtSearchfor" runat="server" maxlength="254" columns="60"></asp:textbox>&nbsp;
				<asp:button id="btnSearch" runat="server" text="Button" cssclass="forum"></asp:button></td>
		</tr>
	</table>
</asp:panel>
<asp:panel id="pnlSearchResult" runat="server" visible="False">
	<table class="grid" id="tblSearchresult" cellspacing="1" cellpadding="1" width="100%"
		border="0">
		<tr class="gridsubheader">
			<td colspan="4">
				<asp:label id="lblSearchresult" runat="server">Label</asp:label></td>
		</tr>
		<asp:repeater id="rptSearchresult" runat="server">
			<itemtemplate>
				<tr class="forumrow">
					<td width="1%" valign="top" class="newcolum">
						&nbsp;</td>
					<td width="50%" class="forumcolumn">
						<%# GetForumPostLink(Container.DataItem) %>
					</td>
					<td width="19%" valign="top" class="forumcolumn">
						<%# DataBinder.Eval(Container.DataItem, "UserName") %>
					</td>
					<td width="30%" valign="top" class="forumcolumn">
						<%# DataBinder.Eval(Container.DataItem, "DateCreated") %>
					</td>
				</tr>
			</itemtemplate>
			<alternatingitemtemplate>
				<tr class="forumrowalt">
					<td width="1%" valign="top" class="newcolumn">
						&nbsp;</td>
					<td width="50%" class="forumcolumn">
						<%# GetForumPostLink(Container.DataItem) %>
					</td>
					<td width="19%" valign="top" class="forumcolumn">
						<%# DataBinder.Eval(Container.DataItem, "UserName") %>
					</td>
					<td width="30%" valign="top" class="forumcolumn">
						<%# DataBinder.Eval(Container.DataItem, "DateCreated") %>
					</td>
				</tr>
			</alternatingitemtemplate>
		</asp:repeater>
	</table>
</asp:panel>
<asp:placeholder id="phForumFooter" runat="server"></asp:placeholder>
