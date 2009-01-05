<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<%@ Page language="c#" Codebehind="EditHtml.aspx.cs" AutoEventWireup="True" Inherits="Cuyahoga.Modules.StaticHtml.EditHtml" ValidateRequest="false" %>
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
			<div id="moduleadminpane">
				<h1>Edit static content</h1>
				<fckeditorv2:fckeditor id="fckEditor" runat="server" height="400px" width="700px"></fckeditorv2:fckeditor>
				<br/>
				<br/>
				<asp:button id="btnSave" runat="server" text="Save" onclick="btnSave_Click"></asp:button>
			</div>
		</form>
	</body>
</html>
