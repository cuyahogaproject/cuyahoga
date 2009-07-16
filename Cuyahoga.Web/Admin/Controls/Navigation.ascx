<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Navigation.ascx.cs"
    Inherits="Cuyahoga.Web.Admin.Controls.Navigation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div class="navsection">
    <h3>
        <asp:Image ImageUrl="../Images/home.gif" runat="server" ImageAlign="left" ID="i1"
            AlternateText="Home"></asp:Image>
        Sites</h3>
    <asp:PlaceHolder ID="plhNodes" runat="server"></asp:PlaceHolder>
    <br />
    <asp:Image ImageUrl="../Images/new.gif" runat="server" ImageAlign="left" ID="inew"
        AlternateText="New Site"></asp:Image><asp:HyperLink ID="hplNew" NavigateUrl="../SiteEdit.aspx?SiteId=-1"
            CssClass="nodelink" runat="server">Add a new site</asp:HyperLink>
</div>
<br />
<div class="navsection">
    <h3>
        <asp:Image ImageUrl="../Images/modules.gif" runat="server" ImageAlign="left" ID="i2"
            AlternateText="Sections"></asp:Image>
        Sections</h3>
    <asp:HyperLink ID="hplSections" NavigateUrl="../Sections.aspx" runat="server">Manage standalone sections</asp:HyperLink>
</div>
<br />
<div class="navsection">
    <h3>
        <asp:Image ImageUrl="../Images/modules.gif" runat="server" ImageAlign="left" ID="i3"
            AlternateText="Modules"></asp:Image>
        Modules</h3>
    <asp:HyperLink ID="hplModules" NavigateUrl="../Modules.aspx" runat="server">Manage modules</asp:HyperLink>
</div>
<br />
<div class="navsection">
    <h3>
        <asp:Image ImageUrl="../Images/docs.gif" runat="server" ImageAlign="left" ID="i4"
            AlternateText="Templates"></asp:Image>
        Templates</h3>
    <asp:HyperLink ID="hplTemplates" NavigateUrl="../Templates.aspx" runat="server">Manage templates</asp:HyperLink>
</div>
<br />
<div class="navsection">
    <h3>
        <asp:Image ImageUrl="../Images/user.gif" runat="server" ImageAlign="left" ID="i5"
            AlternateText="Users"></asp:Image>
        Users
    </h3>
    <asp:HyperLink ID="hplUsers" NavigateUrl="../Users.aspx" runat="server">Manage users</asp:HyperLink>
</div>
<br />
<div class="navsection">
    <h3>
        <asp:Image ImageUrl="../Images/users.gif" runat="server" ImageAlign="left" ID="i6"
            AlternateText="Roles"></asp:Image>
        Roles
    </h3>
    <asp:HyperLink ID="hplRoles" NavigateUrl="../Roles.aspx" runat="server">Manage roles</asp:HyperLink>
</div>
<br />
<div class="navsection">
    <h3>
        <asp:Image ImageUrl="../Images/search.gif" runat="server" ImageAlign="left" ID="i7"
            AlternateText="FullText index"></asp:Image>
        Search
    </h3>
    <asp:HyperLink ID="hplRebuild" NavigateUrl="../RebuildIndex.aspx" runat="server">Rebuild fulltext index</asp:HyperLink>
</div>
<br />
