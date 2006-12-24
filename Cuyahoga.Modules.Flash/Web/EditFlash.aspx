<%@ Page language="c#" Codebehind="EditFlash.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Flash.Web.EditFlash" ValidateRequest="false" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Edit Alternate static content</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Edit&nbsp;alternate static&nbsp;content</h1>
				<fckeditorv2:fckeditor id="fckEditor" runat="server" width="700px" height="400px"></fckeditorv2:fckeditor><br>
				<br>
				<asp:button id="btnSave" runat="server" text="Save"></asp:button><asp:label id="lblMessage" runat="server" EnableViewState="False">Label</asp:label></div>
		</form>
	</body>
</html>
