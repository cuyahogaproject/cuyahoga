<%@ Control Language="c#" AutoEventWireup="false" Codebehind="GalleryComments.ascx.cs" Inherits="Cuyahoga.Modules.Gallery.Web.GalleryComments" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="cc1" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<asp:Panel ID="pnlAll" Runat="server">
	<A id="Comments" name="Comments"></A>
	<H3>
		<asp:Literal id="litTitle" runat="server"></asp:Literal></H3>
	<TABLE cellSpacing="2" cellPadding="2" width="100%" border="0">
		<TR>
			<TD vAlign="middle" align="center" width="20%">
				<asp:Image id="imgGallery" Runat="server"></asp:Image></TD>
			<TD vAlign="top" align="left">
				<P class="xpDescription">
					<asp:Literal id="litDescription" runat="server"></asp:Literal></P>
			</TD>
		</TR>
	</TABLE>
	<H3><%= base.GetText("COMMENTS") %></H3>
	<UL class="xpComments">
		<asp:repeater id="rptComments" runat="server">
			<itemtemplate>
				<li>
					<p><%# DataBinder.Eval(Container.DataItem, "CommentText") %></p>
					<div class="xpCommentSub">
						<%= base.GetText("BY") %>
						<asp:placeholder id="plhCommentBy" runat="server"></asp:placeholder>
						-
						<asp:literal id="litUpdateTimestamp" runat="server"></asp:literal>
					</div>
				</li>
			</itemtemplate>
		</asp:repeater></UL>
	<DIV class="pager">
		<cc1:pager id="pgrComments" runat="server" controltopage="rptComments" cachedatasource="True"
			pagesize="10" cacheduration="30" cachevarybyparams="SectionId"></cc1:pager></DIV>
	<asp:panel id="pnlComment" runat="server" visible="False">
		<asp:panel id="pnlAnonymous" runat="server" visible="False">
			<%= base.GetText("NAME") %>
			<BR>
			<asp:textbox id="txtName" runat="server" width="450px" maxlength="100"></asp:textbox>
			<asp:requiredfieldvalidator id="rfvName" runat="server" cssclass="xperror" enableclientscript="False" controltovalidate="txtName"
				display="Dynamic"></asp:requiredfieldvalidator>
			<BR>
			<%= base.GetText("WEBSITE") %>
			<BR>
			<asp:textbox id="txtWebsite" runat="server" width="450px" maxlength="100"></asp:textbox>
		</asp:panel>
		<%= base.GetText("COMMENTS") %>
		<BR>
		<asp:textbox id="txtComment" runat="server" width="450px" textmode="MultiLine" height="150px"></asp:textbox>
		<asp:requiredfieldvalidator id="rfvComment" runat="server" cssclass="xperror" enableclientscript="False" controltovalidate="txtComment"
			display="Dynamic"></asp:requiredfieldvalidator>
		<asp:label id="lblError" runat="server" visible="False" cssclass="xperror"></asp:label>
		<BR>
		<asp:button id="btnSaveComment" runat="server"></asp:button>
	</asp:panel>
</asp:Panel>
