<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Nav2.ascx.cs" Inherits="Cuyahoga.Web.Templates.Controls.Nav2" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<ul class="submenu">
	<asp:repeater id="rptNav2" runat="server" enableviewstate="False">
		<itemtemplate>
			<li><asp:hyperlink id="hplNav2" runat="server" cssclass="submenulink"></asp:hyperlink></li>
		</itemtemplate>
	</asp:repeater>
</ul>
