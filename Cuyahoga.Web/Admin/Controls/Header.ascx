<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Header.ascx.cs" Inherits="Cuyahoga.Web.Admin.Controls.Header" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="header">
	<div id="headertitle">
		Cuyahoga Site Administration
	</div>
	<div id="headeruser">
	</div>
</div>
<div id="subheader">
	[<asp:hyperlink id="hplSite" runat="server">View the current site</asp:hyperlink>] 
	[<asp:linkbutton id="lbtLogout" runat="server">Log out</asp:linkbutton>]
</div>
