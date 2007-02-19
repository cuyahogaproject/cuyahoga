<%@ register tagprefix="cc1" namespace="Cuyahoga.ServerControls" assembly="Cuyahoga.ServerControls" %>
<%@ control language="c#" autoeventwireup="false" codebehind="ForumReplyPost.ascx.cs"
	inherits="Cuyahoga.Modules.Forum.ForumReplyPost" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:placeholder id="phForumTop" runat="server"></asp:placeholder>
<asp:panel id="pnlForumReplyPost" runat="server" cssclass="articlelist">
	<asp:literal id="ltJsInject" runat="server"></asp:literal>
	<table class="grid" id="tblTopTable">
		<tr class="forumrow">
			<td class="gridsubheader" width="100%" colspan="2">
				<asp:label id="lblReplyTopic" runat="server">Reply to topic</asp:label></td>
		</tr>
		<tr>
			<td valign="top" width="25%">
				<asp:label id="lblOrigTopic" runat="server">Original messages</asp:label>
				<td width="75%">
					<asp:panel id="pnlOrigPost" runat="server" backcolor="GhostWhite" bordercolor="Gray"
						borderwidth="1px" borderstyle="Solid">
						<asp:literal id="ltOrigPost" runat="server"></asp:literal>
					</asp:panel>
				</td>
		</tr>
		<tr>
			<td valign="top" width="25%">
				<asp:label id="lblPreview" runat="server" visible="False">Preview</asp:label>
				<td valign="top" width="75%">
					<asp:panel id="pnlPreview" runat="server" backcolor="#E0E0E0" bordercolor="DarkGray"
						borderwidth="1px" borderstyle="Solid" visible="False">
						<asp:literal id="ltPreviewPost" runat="server" visible="False"></asp:literal>
					</asp:panel>
				</td>
		</tr>
		<tr>
			<td width="100%" colspan="2">
				&nbsp;</td>
		</tr>
		<tr>
			<td valign="top" width="25%">
				<asp:literal id="ltMessage" runat="server" visible="False" Text="Message"></asp:literal>
				<asp:panel id="pnlSmily" runat="server">
					<asp:repeater id="rptSmily" runat="server" enableviewstate="False">
						<itemtemplate>
							<%# GetEmoticonIcon(Container.DataItem) %>
						</itemtemplate>
					</asp:repeater>
				</asp:panel>
			</td>
			<td width="75%">
				<asp:textbox id="txtMessage" runat="server" cssclass="forum" enableviewstate="False"
					columns="70" rows="20" textmode="MultiLine"></asp:textbox>
				<asp:requiredfieldvalidator id="rfvMessage" runat="server" enableclientscript="False"
					controltovalidate="txtMessage" errormessage="Message is required"></asp:requiredfieldvalidator></td>
		</tr>
		<tr>
			<td valign="top" width="25%">
				<asp:label id="llbAttachFile" runat="server" visible="True">Attach file</asp:label></td>
			<td valign="top" width="75%">
				<input class="forum" id="txtAttachment" style="width: 300px" type="file" name="filUpload"
					runat="server"></td>
		</tr>
		<tr>
			<td align="center" width="100%" colspan="2">
				<asp:button id="btnPreview" runat="server" cssclass="forum" text="Preview"></asp:button>&nbsp;
				<asp:button id="btnPost" runat="server" cssclass="forum" text="Post"></asp:button>&nbsp;
				<asp:button id="btnCancel" runat="server" cssclass="forum" text="Cancel" causesvalidation="False">
				</asp:button></td>
		</tr>
	</table>
	<asp:panel id="pnlUploadError" runat="server" visible="False">
		<asp:literal id="ltlUploadError" runat="server"></asp:literal>
	</asp:panel>
</asp:panel>
<asp:placeholder id="phForumFooter" runat="server"></asp:placeholder>
