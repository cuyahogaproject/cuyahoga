<%@ Page language="c#" Codebehind="TemplateEdit.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.TemplateEdit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>TemplateEdit</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<p>
				<em>Make sure you have placed at least one template control (.ascx) in the 
					directory specified as the Base path and at least one css file in Base path/Css 
					directory.</em>
			</p>
			<div class="group">
				<h4>General</h4>
				<table>
					<tr>
						<td style="WIDTH: 200px">Name</td>
						<td><asp:textbox id="txtName" runat="server" width="200px"></asp:textbox>
							<asp:requiredfieldvalidator id="rfvName" runat="server" errormessage="Name is required" cssclass="validator"
								display="Dynamic" enableclientscript="False" controltovalidate="txtName"></asp:requiredfieldvalidator>
						</td>
					</tr>
					<tr>
						<td>Base path (from app root, without beginning '/')</td>
						<td><asp:textbox id="txtBasePath" runat="server" width="200px"></asp:textbox>
							<asp:button id="btnVerifyBasePath" runat="server" text="Verify" causesvalidation="False"></asp:button>
							<asp:requiredfieldvalidator id="rfvBasePath" runat="server" errormessage="Base path is required" cssclass="validator"
								display="Dynamic" enableclientscript="False" controltovalidate="txtBasePath"></asp:requiredfieldvalidator>
						</td>
					</tr>
					<tr>
						<td>Template control</td>
						<td>
							<asp:dropdownlist id="ddlTemplateControls" runat="server"></asp:dropdownlist>
							<asp:label id="lblTemplateControlWarning" runat="server" cssclass="validator" visible="False"
								enableviewstate="False"></asp:label></td>
					</tr>
					<tr>
						<td>Css</td>
						<td><asp:dropdownlist id="ddlCss" runat="server"></asp:dropdownlist>
							<asp:label id="lblCssWarning" runat="server" cssclass="validator" visible="False" enableviewstate="False"></asp:label>
						</td>
					</tr>
				</table>
			</div>
			<br/>
			<asp:panel id="pnlPlaceholders" runat="server" visible="False" cssclass="group">
				<h4>Placeholders</h4>
				<table class="tbl">
					<tr>
						<th>
							Placeholder</th>
						<th>
							Attached section</th>
						<th>
						</th>
					</tr>
					<asp:repeater id="rptPlaceholders" runat="server">
						<itemtemplate>
							<tr>
								<td>
									<asp:label id="lblPlaceholder" runat="server"></asp:label></td>
								<td>
									<asp:hyperlink id="hplSection" runat="server" visible="False"></asp:hyperlink></td>
								<td>
									<asp:hyperlink id="hplAttachSection" runat="server" visible="false">Attach section</asp:hyperlink>
									<asp:linkbutton id="lbtDetachSection" runat="server" visible="false" commandname="detach">Detach section</asp:linkbutton>
								</td>
							</tr>
						</itemtemplate>
					</asp:repeater></table>
			</asp:panel>
			<br/>
			<asp:button id="btnSave" runat="server" text="Save"></asp:button>
			<asp:button id="btnBack" runat="server" text="Back" causesvalidation="false"></asp:button>
			<asp:button id="btnDelete" runat="server" text="Delete" causesvalidation="false"></asp:button>
		</form>
	</body>
</html>
