<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Downloads.ascx.cs" Inherits="Cuyahoga.Modules.Downloads.Web.Downloads" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<ul class="articlelist">
	<asp:repeater id="rptFiles" runat="server" enableviewstate="False">
		<itemtemplate>
			<li>
				<h4>
				<asp:hyperlink id="hplFileImg" runat="server"></asp:hyperlink>
				<asp:hyperlink id="hplFile" runat="server">
					<%# Eval("Title") %>
				</asp:hyperlink>
				</h4>
				<asp:panel id="pnlFileDetails" cssclass="articlesub" visible="False" runat="server">
					<asp:label id="lblDateModified" runat="server" visible="False">
					</asp:label>
					<asp:label id="lblPublisher" runat="server" visible="False">
					</asp:label>
					<asp:label id="lblNumberOfDownloads" runat="server" visible="False">
					</asp:label>
				</asp:panel>
			</li>
		</itemtemplate>
	</asp:repeater>
</ul>