<%@ Control Language="c#" AutoEventWireup="True" Codebehind="LanguageSwitcher.ascx.cs" Inherits="Cuyahoga.Modules.LanguageSwitcher.LanguageSwitcher" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="languageswitcher">
	<asp:panel id="pnlLinks" runat="server" visible="False">
		<asp:placeholder id="plhLanguageLinks" runat="server"></asp:placeholder>
	</asp:panel>
	<asp:panel id="pnlDropDown" runat="server" visible="False">
		<asp:dropdownlist id="ddlLanguage" runat="server" width="120px"></asp:dropdownlist>
		<asp:imagebutton id="imbGo" runat="server" causesvalidation="False"></asp:imagebutton>
	</asp:panel>
</div>