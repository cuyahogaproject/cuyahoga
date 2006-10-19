<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Navigation.ascx.cs" Inherits="Cuyahoga.Web.Admin.Controls.Navigation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="navsection">
	<h3>
		<asp:image imageurl="../Images/home.gif" runat="server" imagealign="left" id="i1" alternatetext="Home"></asp:image>
		Sites</h3>
	<asp:placeholder id="plhNodes" runat="server"></asp:placeholder>
	<br/>
	<asp:image imageurl="../Images/new.gif" runat="server" imagealign="left" id="inew" alternatetext="New Site"></asp:image><asp:hyperlink id="hplNew" navigateurl="../SiteEdit.aspx?SiteId=-1" cssclass="nodelink" runat="server">Add a new site</asp:hyperlink>
</div>
<br/>
<div class="navsection">
	<h3>
		<asp:image imageurl="../Images/modules.gif" runat="server" imagealign="left" id="i2" alternatetext="Sections"></asp:image>
		Sections</h3>
	<asp:hyperlink id="hplSections" navigateurl="../Sections.aspx" runat="server">Manage standalone sections</asp:hyperlink>
</div>
<br/>
<div class="navsection">
	<h3>
		<asp:image imageurl="../Images/modules.gif" runat="server" imagealign="left" id="i3" alternatetext="Modules"></asp:image>
		Modules</h3>
	<asp:hyperlink id="hplModules" navigateurl="../Modules.aspx" runat="server">Manage modules</asp:hyperlink>
</div>
<br/>
<div class="navsection">
	<h3>
		<asp:image imageurl="../Images/docs.gif" runat="server" imagealign="left" id="i4" alternatetext="Templates"></asp:image>
		Templates</h3>
	<asp:hyperlink id="hplTemplates" navigateurl="../Templates.aspx" runat="server">Manage templates</asp:hyperlink>
</div>
<br/>
<div class="navsection">
	<h3>
		<asp:image imageurl="../Images/user.gif" runat="server" imagealign="left" id="i5" alternatetext="Users"></asp:image>
		Users
	</h3>
	<asp:hyperlink id="hplUsers" navigateurl="../Users.aspx" runat="server">Manage users</asp:hyperlink>
</div>
<br/>
<div class="navsection">
	<h3>
		<asp:image imageurl="../Images/users.gif" runat="server" imagealign="left" id="i6" alternatetext="Roles"></asp:image>
		Roles
	</h3>
	<asp:hyperlink id="hplRoles" navigateurl="../Roles.aspx" runat="server">Manage roles</asp:hyperlink>
</div>
<br/>
<div class="navsection">
	<h3>
		<asp:image imageurl="../Images/search.gif" runat="server" imagealign="left" id="i7" alternatetext="FullText index"></asp:image>
		Search
	</h3>
	<asp:hyperlink id="hplRebuild" navigateurl="../RebuildIndex.aspx" runat="server">Rebuild fulltext index</asp:hyperlink>
</div>
