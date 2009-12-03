<%@ Control Language="c#" AutoEventWireup="false" Inherits="Cuyahoga.Web.UI.BaseTemplate" %>
<%@ Register TagPrefix="uc1" TagName="navigation" Src="~/Controls/Navigation/HierarchicalMenu.ascx" %>
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
				<div id="searcharea"><!-- searchinput" --></div>
				<div>
					<span id="titletext">Cuyahoga</span>
					A .NET Web Site Framework
				</div>
			</div>
			
			<!-- shadow divs -->
			<div id="containerleft">
			<div id="containertopleft">
			<div id="containerright">
			<div id="containertopright">
			<!-- main -->
			<div id="main">
				<div id="globalmenu">
					<!-- globalMenu" -->
				</div>
				<div id="side">
					<div id="nav">
						<!-- Nav" -->
					</div>
					<div id="sidecontent">
						<!-- side1content -->
					</div>
				</div>
				<div id="content">
					<!-- maincontent -->
					<h5>This page is currently offline for maintenance.</h5>
					<a href="/">Back to home</a>
				</div>
				<div class="clear"></div>
			</div>
			<!-- end main -->
			</div>
			</div>
			</div>
			</div>
			<!-- end shadow divs -->
		</div>
	</form>
</body>
</html>

