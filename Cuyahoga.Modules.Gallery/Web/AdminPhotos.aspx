<%@ Page language="c#" Codebehind="AdminPhotos.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Gallery.Web.AdminPhotos" ValidateRequest="false" %>
<%@ Register TagPrefix="uc2" TagName="ImageTable" src="ImageTable.ascx"%>
<%@ Register TagPrefix="uc1" TagName="ImageManager" src="ImageManager.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AdminPhotos</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Manage Gallery Photos</h1>
				<h4>Photos in Gallery: &nbsp;<asp:literal id="litGalleryName" runat="server"></asp:literal></h4>
				<uc2:imagetable id="imgTable" runat="server" EnableViewState="False"></uc2:imagetable><br>
				<input id="btnOrganize" disabled type="button" value="Organize photos order" runat="server">
				<H4>Selected Photo</H4>
				<input id="btnNew" type="button" value="New Photo" name="btnNew" runat="server">
				<asp:button id="btnSave" runat="server" Text="Save Photo"></asp:button><asp:button id="btnDelete" runat="server" Text="Delete Photo" Enabled="False"></asp:button><input id="btnBack" type="button" value="Back to admin page" name="btnCancel" runat="server">
				<table class="tbl" border="0">
					<tr>
						<td vAlign="top" align="left" width="50%">
							<table class="tbl">
								<tr>
									<td width="100">Name</td>
									<td><asp:textbox id="txtTitle" runat="server" MaxLength="50" width="300px"></asp:textbox><asp:requiredfieldvalidator id="rfvTitle" runat="server" cssclass="validator" display="Dynamic" errormessage="The title is required"
											enableclientscript="False" controltovalidate="txtTitle"></asp:requiredfieldvalidator></td>
								</tr>
								<tr>
									<td width="100">Serie</td>
									<td><asp:textbox id="txtSerie" runat="server" MaxLength="255" width="300px"></asp:textbox></td>
								</tr>
								<tr>
									<td width="100">Category</td>
									<td><asp:textbox id="txtCategory" runat="server" MaxLength="255" width="300px"></asp:textbox></td>
								</tr>
								<tr>
									<td width="100">Original Size</td>
									<td><asp:textbox id="txtOriginalSize" runat="server" MaxLength="255" width="300px"></asp:textbox></td>
								</tr>
								<tr>
									<td colSpan="2">
										<table cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
											<tr>
												<td align="center">Thumb<br>
													<uc1:imagemanager id="imgManager1" runat="server"></uc1:imagemanager></td>
												<td>Thumb Roll over<br>
													<uc1:imagemanager id="imgManager2" runat="server"></uc1:imagemanager></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td colSpan="2">Descriptions</td>
								</tr>
								<asp:repeater id="rptDesciptions" runat="server">
									<ItemTemplate>
										<tr>
											<td width="100"><%# DataBinder.Eval(Container.DataItem, "Culture")%></td>
											<td>
												<textarea name='<%# DataBinder.Eval(Container.DataItem, "Culture")%>' style="WIDTH: 300px; HEIGHT: 60px"><%# DataBinder.Eval(Container.DataItem, "Description")%></textarea>
											</td>
										</tr>
									</ItemTemplate>
								</asp:repeater></table>
						</td>
						<td vAlign="top" align="center"><asp:panel id="pnlImageProcessing" Runat="server">
								<TABLE class="tbl" width="100%" border="1">
									<TBODY>
										<TR>
											<TD align="center" colSpan="3"><B>Image Processing</B></TD>
										</TR>
										<TR>
											<TD width="100">Thumbnails</TD>
											<TD align="left" width="250"><asp:checkbox id="cbgenThumb" runat="server" Text="generate" TextAlign="Left"></asp:checkbox>&nbsp;
												<asp:checkbox id="cbRatio" runat="server" Text="preserve ratio" TextAlign="Left"></asp:checkbox><BR>
												width&nbsp;
												<asp:textbox id="txtWidth" runat="server" MaxLength="3" Width="50px"></asp:textbox>height&nbsp;
												<asp:textbox id="txtHeight" runat="server" MaxLength="3" Width="50px"></asp:textbox></TD>
											<TD width="50"><INPUT id="brnGenerate" type="button" value="Go" name="brnGenerate" runat="server">
											</TD>
										</TR>
										<TR>
											<TD>Watermark</TD>
											<TD align="left"><asp:textbox id="txtWatermark" runat="server" MaxLength="255" Width="250px"></asp:textbox><BR>
												font size&nbsp;
												<asp:textbox id="txtFontsize" runat="server" MaxLength="3" Width="50px"></asp:textbox></TD></td>
						<TD width="50"><INPUT id="btnWatermark" type="button" value="Go" name="btnWatermark" runat="server">
						</TD>
					</tr>
				</table>
				</asp:panel>Attach large Image<br>
				<uc1:imagemanager id="imgManager3" runat="server"></uc1:imagemanager></TD></TD></TR></TBODY></TABLE></div>
		</form>
	</body>
</HTML>
