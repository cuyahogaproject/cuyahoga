<%@ Register TagPrefix="uc2" TagName="ImageTable" src="ImageTable.ascx"%>
<%@ Control Language="c#" AutoEventWireup="false" Codebehind="PhotoList.ascx.cs" Inherits="Cuyahoga.Modules.Gallery.Web.PhotoList" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<h3><asp:literal id="litGalleryTitle" runat="server"></asp:literal></h3>
<asp:panel id="pnlDescription" runat="server" Visible="False">
	<P class="plaintext_black">
		<asp:Literal id="litDescription" runat="server"></asp:Literal></P>
	<HR class="xpGalleryDivider">
</asp:panel><uc2:imagetable id="imgTable" runat="server" EnableViewState="False"></uc2:imagetable>
<asp:panel id="pnlRating" Runat="server">
	<DIV style="LEFT: 0px; WIDTH: 83px; POSITION: absolute; TOP: 0px; HEIGHT: 18px"><IMG height="7" src="{0}" width="{1}" border="0">
	</DIV>
	<DIV style="Z-INDEX: 1; LEFT: 0px; WIDTH: 83px; POSITION: absolute; TOP: 0px; HEIGHT: 18px"><IMG height="7" src="{2}" width="60" border="0">
	</DIV>
</asp:panel>
