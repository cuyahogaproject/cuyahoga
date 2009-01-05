<%@ Control Language="c#" AutoEventWireup="false" Codebehind="NavigationLevelZeroOne.ascx.cs" Inherits="Cuyahoga.Web.Templates.Controls.NavigationLevelZeroOne" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="mainmenuarea">
	<ul id="mainmenu">
		<li><asp:hyperlink id="hplHome" runat="server"></asp:hyperlink></li>
			<asp:repeater id="rptNav1" runat="server" enableviewstate="False">
				<itemtemplate>
					<li><asp:hyperlink id="hplNav1" runat="server"></asp:hyperlink></li>
				</itemtemplate>
			</asp:repeater>
		<li id="liAdmin" runat="server" visible="false"><asp:hyperlink id="hplAdmin" runat="server" visible="False">Admin</asp:hyperlink></li>
	</ul>
</div>
