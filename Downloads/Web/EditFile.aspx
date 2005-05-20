<%@ Page language="c#" Codebehind="EditFile.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Downloads.Web.EditFile" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>EditFile</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body ms_positioning="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Edit File</h1>
				<div class="group">
					<h4>File properties</h4>
					<table>
					</table>
				</div>
				<p>
					<asp:button id="btnSave" runat="server" text="Save"></asp:button>
					<asp:button id="btnDelete" runat="server" text="Delete" visible="False" causesvalidation="False"></asp:button>
					<input id="btnCancel" type="button" value="Cancel" runat="server" name="btnCancel">
				</p>
			</div>
		</form>
	</body>
</html>
