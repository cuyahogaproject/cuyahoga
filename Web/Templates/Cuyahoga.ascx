<%@ Control Language="c#" AutoEventWireup="false" Inherits="Cuyahoga.Web.UI.BaseTemplate" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="navigation" Src="../Controls/Nav1.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 TRANSITIONAL//EN" >
<html>
<head>
	<title><asp:literal id="PageTitle" runat="server"></asp:literal></title>
	<link id="CssStyleSheet" rel="stylesheet" type="text/css" runat="server" />
</head>

<body>
<form id="t" method="post" runat="server">
<div id="container">
	<div id="header">
		<h1>www.martijn.boland.org</h1>
		<uc1:navigation id="Nav1" runat="server"></uc1:navigation>
	</div>
	<div id="control">
		<asp:placeholder id="secundary" runat="server"></asp:placeholder>
	</div>
	<div id="main">
		<asp:placeholder id="primary" runat="server"></asp:placeholder>
	</div>
	<div id="footer">
		<a href="http://www.go-mono.org" target="_blank"><img src="../Images/mono-powered-big.gif" alt="Powered by mono" border="0"/></a>
	</div>
</div>
</form>
</body>
</html>