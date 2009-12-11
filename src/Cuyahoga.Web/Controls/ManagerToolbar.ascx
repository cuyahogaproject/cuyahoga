<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagerToolbar.ascx.cs" Inherits="Cuyahoga.Web.Controls.ManagerToolbar" %>
<div id="managertoolbar">
	<ul>
		<li><asp:hyperlink id="hplManagePages" runat="server" navigateurl="~/Manager/Pages" visible="false">
			<img src="<%= ResolveUrl("~/Manager/Content/Images/page.png") %>" alt="Manage pages" />Manage pages</asp:hyperlink>
		</li>
		<li><asp:hyperlink id="hplPageContent" runat="server" visible="false">
			<img src="<%= ResolveUrl("~/Manager/Content/Images/pencil.png") %>" alt="Manage page content" />Edit page content</asp:hyperlink>
		</li>
		<li><asp:hyperlink id="hplPageLayout" runat="server" visible="false">
			<img src="<%= ResolveUrl("~/Manager/Content/Images/layout.png") %>" alt="Manage page layout" />Edit page layout</asp:hyperlink>
		</li>
		<li><asp:hyperlink id="hplManageFiles" runat="server" navigateurl="~/Manager/Files" visible="false">
			<img src="<%= ResolveUrl("~/Manager/Content/Images/folder.png") %>" alt="Manage files" />Manage files</asp:hyperlink>
		</li>
		<li><asp:hyperlink id="hplManageUsers" runat="server" navigateurl="~/Manager/Users" visible="false">
			<img src="<%= ResolveUrl("~/Manager/Content/Images/user.png") %>" alt="Manage users and roles" />Manage users</asp:hyperlink>
		</li>
	</ul>
</div>