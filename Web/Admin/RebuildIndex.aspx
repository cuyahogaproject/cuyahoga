<%@ Page language="c#" Codebehind="RebuildIndex.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.RebuildIndex" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>RebuildIndex</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<p>
				<asp:label id="lblMessage" runat="server" visible="False">The index was successfully rebuilt.</asp:label>
			</p>
			<p>
				<asp:button id="btnRebuild" runat="server" text="Rebuild index"></asp:button>
			</p>
			<div id="pleasewait" style="DISPLAY: none">
				Please wait while the index is being rebuild...
			</div>
		</form>
	</body>
</html>
