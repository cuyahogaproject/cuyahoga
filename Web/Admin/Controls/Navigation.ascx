<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Navigation.ascx.cs" Inherits="Cuyahoga.Web.Admin.Controls.Navigation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="navsection">
	<h3>
		<asp:image imageurl="../Images/home.gif" runat="server" imagealign="absmiddle" id="i1"></asp:image>
		Sites</h3>
	<asp:placeholder id="plhNodes" runat="server"></asp:placeholder>
	<br />
	<asp:image imageurl="../Images/new.gif" runat="server" imagealign="absmiddle" id="inew"></asp:image><asp:hyperlink id="hplNew" navigateurl="../SiteEdit.aspx?SiteId=-1" cssclass="nodelink" runat="server">Add a new site</asp:hyperlink>
</div>
<br>
<div class="navsection">
	<h3>
		<asp:image imageurl="../Images/docs.gif" runat="server" imagealign="absmiddle" id="i2"></asp:image>
		Templates</h3>
	<asp:hyperlink id="hplTemplates" navigateurl="../Templates.aspx" runat="server">Manage templates</asp:hyperlink>
</div>
<br>
<div class="navsection">
	<h3>
		<asp:image imageurl="../Images/user.gif" runat="server" imagealign="absmiddle" id="i3"></asp:image>
		Users
	</h3>
	<asp:hyperlink id="hplUsers" navigateurl="../Users.aspx" runat="server">Manage users</asp:hyperlink>
</div>
<br>
<div class="navsection">
	<h3>
		<asp:image imageurl="../Images/users.gif" runat="server" imagealign="absmiddle" id="i4"></asp:image>
		Roles
	</h3>
	<asp:hyperlink id="hplRoles" navigateurl="../Roles.aspx" runat="server">Manage roles</asp:hyperlink>
</div>
<br>
<div class="navsection">
	<h3>
		<asp:image imageurl="../Images/search.gif" runat="server" imagealign="absmiddle" id="i5"></asp:image>
		Search
	</h3>
	<asp:hyperlink id="hplRebuild" navigateurl="../RebuildIndex.aspx" runat="server">Rebuild fulltext index</asp:hyperlink>
</div>
