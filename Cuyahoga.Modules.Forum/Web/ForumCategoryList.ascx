<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ForumCategoryList.ascx.cs" Inherits="Cuyahoga.Modules.Forum.ForumCategoryList" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="cc1" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<asp:PlaceHolder id="phForumTop" runat="server"></asp:PlaceHolder>
<table id="tblCategoryTable" class="grid">
	<asp:repeater id="rptForumList" runat="server">
		<HeaderTemplate>
			<tr class="gridsubheader">
				<td width="1%">&nbsp;</td>
				<td align="left" width="58%"><asp:label id="lblCategoryName" runat="server">Category</asp:label></td>
				<td align="left" width="8%"><asp:label id="lblHdrTopics" runat="server">Topics</asp:label></td>
				<td align="left" width="8%"><asp:label id="lblHdrNumPosts" runat="server">NumPosts</asp:label></td>
				<td align="left" width="25%"><asp:label id="lblHdrLastPost" runat="server">Lastpost</asp:label></td>
			</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr class="forumrow">
				<td width="1%" valign="top">&nbsp;</td>
				<td width="58%"><%# GetForumLink(Container.DataItem) %><br><%# DataBinder.Eval(Container.DataItem, "Description") %></td>
				<td width="8%" valign="top"><%# DataBinder.Eval(Container.DataItem, "NumTopics") %></td>
				<td width="8%" valign="top"><%# DataBinder.Eval(Container.DataItem, "NumPosts") %></td>
				<td width="25%" valign="top"><%# DataBinder.Eval(Container.DataItem, "LastPosted") %></td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="forumrowalt">
				<td width="1%" valign="top">&nbsp;</td>
				<td width="58%" class="forumcolumn"><%# GetForumLink(Container.DataItem) %><br><%# DataBinder.Eval(Container.DataItem, "Description") %></td>
				<td width="8%" valign="top" class="forumcolumn"><%# DataBinder.Eval(Container.DataItem, "NumTopics") %></td>
				<td width="8%" valign="top" class="forumcolumn"><%# DataBinder.Eval(Container.DataItem, "NumPosts") %></td>
				<td width="25%" valign="top" class="lastpostcolumn"><%# DataBinder.Eval(Container.DataItem, "LastPosted") %></td>
			</tr>
		</AlternatingItemTemplate>
	</asp:repeater>
</table>
<asp:PlaceHolder id="phForumFooter" runat="server"></asp:PlaceHolder>
