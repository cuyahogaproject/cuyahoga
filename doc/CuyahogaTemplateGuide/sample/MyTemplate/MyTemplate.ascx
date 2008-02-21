<%@ Control Language="c#" AutoEventWireup="false" Inherits="Cuyahoga.Web.UI.BaseTemplate" %>
<%@ Register TagPrefix="uc1" TagName="navigation" src="~/Controls/Navigation/NavigationLevelZeroOne.ascx" %>
<%@ Register TagPrefix="uc2" TagName="subnavigation" Src="~/Controls/Navigation/NavigationLevelTwo.ascx" %>
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
				<img src='<%= ResolveUrl("~/Templates/MyTemplate/Images/cuyahoga-logo.png") %>' alt="Logo" style="vertical-align:middle" />
				<span id="title">My Cuyahoga site</span>
			</div>
			<div id="mainmenu">
				<uc1:navigation id="Nav1" runat="server"></uc1:navigation>
			</div>
			
			<!-- main -->
			<div id="main">
				<!-- sidebar -->
				<div id="side">
					<div id="submenu">
						<uc2:subnavigation id="Nav2" runat="server"></uc2:subnavigation>
					</div>
					<div id="sidecontent">
						<asp:placeholder id="sidecontent" runat="server"></asp:placeholder>
					</div>
				</div>
				<!-- main content area -->
				<div id="maincontent">
					<asp:placeholder id="maincontent" runat="server"></asp:placeholder>
				</div>
			</div>
			<!-- end main -->
		</div>
	</form>
</body>
</html>