<%@ control language="c#" autoeventwireup="false" codebehind="ForumTop.ascx.cs" inherits="Cuyahoga.Modules.Forum.ForumTop"
	targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ register tagprefix="cc1" namespace="Cuyahoga.ServerControls" assembly="Cuyahoga.ServerControls" %>
<table id="tblTopTable" class="grid" width="100%">
	<tr class="gridsubheader">
		<td width="30%">
			<asp:label id="lblWelcomeText" runat="server">Label</asp:label></td>
		<td width="70%" align="right">
			<asp:hyperlink id="hplSearch" runat="server" cssclass="forum">Search</asp:hyperlink>
			<asp:hyperlink id="hplForumProfile" runat="server" visible="False" cssclass="forum">My profile</asp:hyperlink>&nbsp;</td>
	</tr>
</table>
<table id="tblTopTable2" cellspacing="1" cellpadding="1" width="100%" border="0">
	<tr>
		<td width="100%">
			<asp:hyperlink id="hplForumBreadCrumb" runat="server">HyperLink</asp:hyperlink>
			<asp:label id="lblForward_1" runat="server" visible="False">&nbsp;::&nbsp;</asp:label>
			<asp:hyperlink id="hplCategoryLink" runat="server" visible="False">HyperLink</asp:hyperlink>
			<asp:label id="lblForward_2" runat="server" visible="False">&nbsp;::&nbsp;</asp:label>
			<asp:hyperlink id="hplForumLink" runat="server" visible="False">HyperLink</asp:hyperlink>
			<asp:label id="lblForward_3" runat="server" visible="False">&nbsp;::&nbsp;</asp:label>
			<asp:hyperlink id="hplPostlink" runat="server" visible="False">HyperLink</asp:hyperlink>
		</td>
	</tr>
</table>
