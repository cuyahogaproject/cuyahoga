<%@ control language="c#" autoeventwireup="false" codebehind="ForumNewPost.ascx.cs"
	inherits="Cuyahoga.Modules.Forum.ForumNewPost" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ register tagprefix="cc1" namespace="Cuyahoga.ServerControls" assembly="Cuyahoga.ServerControls" %>
<asp:placeholder id="phForumTop" runat="server"></asp:placeholder>
<asp:panel id="pnlTop" runat="server" cssclass="articlelist">
	<asp:literal id="ltJsInject" runat="server"></asp:literal>
	<table class="grid" id="tblTopTable">
		<tr class="forumrow">
			<td class="gridsubheader" colspan="2">
				<asp:label id="lblNewTopic" runat="server">New topic</asp:label></td>
		</tr>
		<tr>
			<td valign="top" width="25%">
				<asp:label id="lblPreview" runat="server" visible="False">Preview</asp:label></td>
			<td valign="top" width="75%">
				<asp:panel id="pnlPreview" runat="server" visible="False" backcolor="#E0E0E0" bordercolor="Silver"
					borderstyle="Solid" borderwidth="1px" enableviewstate="False">
					<asp:literal id="ltPreviewPost" runat="server" visible="False"></asp:literal>
				</asp:panel>
			</td>
		</tr>
		<tr>
			<td valign="top" width="25%">
				<asp:literal id="ltTopic" runat="server" visible="True"></asp:literal></td>
			<td width="75%">
				<asp:textbox id="txtSubject" runat="server" cssclass="forum" columns="70" maxlength="50"></asp:textbox>
				<asp:requiredfieldvalidator id="rfvSubject" runat="server" errormessage="Topic is required"
					controltovalidate="txtSubject"></asp:requiredfieldvalidator></td>
		</tr>
		<tr>
			<td valign="top" width="25%">
				<asp:literal id="ltMessage" runat="server" visible="True"></asp:literal>
				<asp:panel id="pnlSmily" runat="server" cssclass="forumsmily">
					<asp:repeater id="rptSmily" runat="server">
						<itemtemplate>
							<%# GetEmoticonIcon(Container.DataItem) %>
						</itemtemplate>
					</asp:repeater>
				</asp:panel>
			</td>
			<td valign="top" width="75%">
				<asp:textbox id="txtMessage" runat="server" cssclass="forum" columns="70" textmode="MultiLine"
					rows="20"></asp:textbox>
				<asp:requiredfieldvalidator id="rfvMessage" runat="server" errormessage="Message is required"
					controltovalidate="txtMessage"></asp:requiredfieldvalidator></td>
		</tr>
		<tr>
			<td valign="top" width="25%">
				<asp:label id="llbAttachFile" runat="server" visible="True">Attach file</asp:label></td>
			<td valign="top" width="75%">
				<input class="forum" id="txtAttachment" style="width: 300px" type="file" name="filUpload"
					runat="server" /></td>
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
