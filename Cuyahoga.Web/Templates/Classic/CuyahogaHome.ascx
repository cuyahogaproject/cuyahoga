<%@ Control Language="c#" AutoEventWireup="false" Inherits="Cuyahoga.Web.UI.BaseTemplate" %>
<%@ Register TagPrefix="uc1" TagName="navigation" Src="~/Controls/Navigation/NavigationLevelZeroOne.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title><asp:literal id="PageTitle" runat="server"></asp:literal></title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<asp:literal id="MetaTags" runat="server" />
	<asp:literal id="Stylesheets" runat="server" />
	<asp:literal id="JavaScripts" runat="server" />
	</head>
<body>
<form id="t" method="post" runat="server">
<div id="container">
	<div id="header">
		<div id="logoarea">
			<div id="title">Cuyahoga</div>
			<div id="subtitle">A .NET Website Framework</div>
		</div>
		<uc1:navigation id="Nav1" runat="server"></uc1:navigation>
	</div>
	<div id="content">
		<div id="side1">
			<asp:placeholder id="side1content" runat="server"></asp:placeholder>	
		</div>
		<div id="main">
			<asp:placeholder id="maincontent" runat="server"></asp:placeholder>	
		</div>
	</div>
</div>
</form>
</body>
</html>
