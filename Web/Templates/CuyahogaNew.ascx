<%@ Control Language="c#" AutoEventWireup="false" Inherits="Cuyahoga.Web.UI.BaseTemplate" %>
<%@ Register TagPrefix="uc1" TagName="navigation" Src="Controls/Nav.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<title><asp:literal id="PageTitle" runat="server"></asp:literal></title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<link id="CssStyleSheet" rel="stylesheet" type="text/css" runat="server" />
</head>
<body>
	<form id="t" method="post" runat="server">
		<div id="container">
			<div id="side">
				<div id="headerside">
					<span id="title">Cuyahoga</span>
				</div>
				<div id="nav">
					<uc1:navigation id="Nav" runat="server"></uc1:navigation>
				</div>
				<div id="sidecontent">
					<asp:placeholder id="side1content" runat="server"></asp:placeholder>
				</div>
			</div>
			<div id="main">
				<div id="headermain">
					<span id="subtitle">A .NET Website Framework</span>
				</div>
				<div id="content">
					<asp:placeholder id="maincontent" runat="server"></asp:placeholder>
				</div>
			</div>
		</div>
	</form>
</body>
</html>

