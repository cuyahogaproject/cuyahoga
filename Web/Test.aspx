<%@ Register TagPrefix="cc1" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<%@ Page language="c#" Codebehind="Test.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Test" %>
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
			<p><cc1:calendar id="cal1" runat="server" width="150px" theme="blue" selecteddate="2004-09-01" displaytime="True"></cc1:calendar></p>
			<p><asp:button id="Button1" runat="server" text="Check Date(Time)"></asp:button></p>
			<p>
				<asp:label id="Label1" runat="server"></asp:label></p>
			<p>&nbsp;</p>
		</form>
	</body>
</html>
