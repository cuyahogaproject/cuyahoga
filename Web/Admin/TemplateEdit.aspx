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
				<em>Make sure you have placed at least one physical css file in the Css directory 
					that
					is configured in the web.config.</em>
			</p>
			<div class="group">
				<h4>General</h4>
				<table>
					<tr>
						<td style="WIDTH: 200px">Name</td>
						<td><asp:textbox id="txtName" runat="server" width="200px"></asp:textbox>
							<asp:requiredfieldvalidator id="rfvName" runat="server" errormessage="Name is required" cssclass="validator" display="Dynamic" enableclientscript="False" controltovalidate="txtName"></asp:requiredfieldvalidator>
						</td>
					</tr>
					<tr>
						<td>Path</td>
						<td><asp:textbox id="txtPath" runat="server" width="200px"></asp:textbox>
							<asp:requiredfieldvalidator id="rfvPath" runat="server" errormessage="Path is required" cssclass="validator" display="Dynamic" enableclientscript="False" controltovalidate="txtPath"></asp:requiredfieldvalidator>
						</td>
					</tr>
					<tr>
						<td>Css</td>
						<td><asp:dropdownlist id="ddlCss" runat="server"></asp:dropdownlist>
						</td>
					</tr>
				</table>
			</div>
			<br>
			<asp:button id="btnSave" runat="server" text="Save"></asp:button>
			<asp:button id="btnCancel" runat="server" text="Cancel" causesvalidation="false"></asp:button>
			<asp:button id="btnDelete" runat="server" text="Delete" causesvalidation="false"></asp:button>
		</form>

	</body>
</html>
