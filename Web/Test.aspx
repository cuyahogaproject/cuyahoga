<%@ Page language="c#" Codebehind="Test.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Test" %>
<%@ Register TagPrefix="cc1" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Test</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<p>
				<cc1:calendar id="cal1" runat="server" theme="win2k_1"></cc1:calendar></p>
			<p>
				<cc1:calendar id="cal2" runat="server" theme="winter"></cc1:calendar></p>
		</form>
	</body>
</html>
