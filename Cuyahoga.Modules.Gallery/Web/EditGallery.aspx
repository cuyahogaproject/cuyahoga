<%@ Page language="c#" Codebehind="EditGallery.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Gallery.Web.EditGallery" ValidateRequest="false"%>
<%@ Register TagPrefix="uc1" TagName="ImageManager" src="ImageManager.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>EditGallery</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Manage Gallery</h1>
				<asp:panel id="pnlInfo" runat="server" Visible="True">
					<H4>Gallery Information</H4>
					<TABLE class="tbl">
						<TR>
							<TD style="WIDTH: 100px">Include</TD>
							<TD>
								<asp:CheckBox id="cbInclude" runat="server"></asp:CheckBox><SPAN style="PADDING-RIGHT: 20px; PADDING-LEFT: 20px; TEXT-ALIGN: left">Sequence</SPAN>
								<asp:TextBox id="txtSequence" runat="server" MaxLength="3" Columns="3"></asp:TextBox>
								<asp:RegularExpressionValidator id="revSequence" runat="server" ErrorMessage="Position is numeric" CssClass="validator"
									Display="Dynamic" ControlToValidate="txtSequence" EnableClientScript="False" ValidationExpression="\d?"></asp:RegularExpressionValidator></TD>
						<TR>
							<TD style="WIDTH: 100px">Name</TD>
							<TD>
								<asp:textbox id="txtName" runat="server" MaxLength="50" width="650px"></asp:textbox>
								<asp:requiredfieldvalidator id="rfvName" runat="server" cssclass="validator" display="Dynamic" errormessage="The namee is required"
									enableclientscript="False" controltovalidate="txtName"></asp:requiredfieldvalidator></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px">Title</TD>
							<TD>
								<asp:textbox id="txtTitle" runat="server" MaxLength="50" width="650px"></asp:textbox>
								<asp:requiredfieldvalidator id="rfvTitle" runat="server" cssclass="validator" display="Dynamic" errormessage="The title is required"
									enableclientscript="False" controltovalidate="txtTitle"></asp:requiredfieldvalidator></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px">Artist</TD>
							<TD>
								<asp:textbox id="txtArtist" runat="server" width="650px"></asp:textbox></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px">Virtual Path</TD>
							<TD>
								<asp:textbox id="txtVirtualPath" runat="server" width="650px"></asp:textbox></TD>
						</TR>
						<TR>
							<TD style="WIDTH: 100px">Thumb</TD>
							<TD>
								<uc1:ImageManager id="imgManager" runat="server"></uc1:ImageManager></TD>
						</TR>
					</TABLE>
				</asp:panel><asp:panel id="pnlDesciption" runat="server" Visible="True">
					<H4>Gallery Desciptions</H4>
					<TABLE class="tbl">
						<asp:Repeater id="rptDesciptions" runat="server">
							<ItemTemplate>
								<tr>
									<td style="WIDTH: 100px"><%# DataBinder.Eval(Container.DataItem, "Culture")%></td>
									<td>
										<textarea name='<%# DataBinder.Eval(Container.DataItem, "Culture")%>' style="WIDTH: 650px; HEIGHT: 60px"><%# DataBinder.Eval(Container.DataItem, "Description")%></textarea>
									</td>
								</tr>
							</ItemTemplate>
						</asp:Repeater></TABLE>
				</asp:panel>
				<p><asp:button id="btnSave" runat="server" text="Save"></asp:button><asp:button id="btnDelete" runat="server" text="Delete" visible="False"></asp:button><input id="btnCancel" type="button" value="Cancel" name="btnCancel" runat="server">
				</p>
			</div>
		</form>
	</body>
</HTML>
