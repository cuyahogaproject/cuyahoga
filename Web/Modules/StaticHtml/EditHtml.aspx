<%@ Page language="c#" Codebehind="EditHtml.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Modules.StaticHtml.EditHtml" ValidateRequest="false"  %>
<%@ Register TagPrefix="cc1" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<%@ Register TagPrefix="ftb" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>EditHtml</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<h1>Edit static content</h1><cc1:cuyahogaeditor id="cedStaticHtml" runat="server" supportfolder="~/Support/FreeTextBox/" buttonoverimage="True" buttonrendermode="Css" downlevelcols="80" downlevelrows="20" width="700px" height="400px" imagedir="~/UserFiles/Images"></cc1:cuyahogaeditor><br><br><asp:button id="btnSave" runat="server" text="Save"></asp:button></form>

	</body>
</html>
