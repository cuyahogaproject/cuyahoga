<%@ Control Language="c#" AutoEventWireup="false" Inherits="Cuyahoga.Web.UI.BaseTemplate" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 TRANSITIONAL//EN" >
<html>
  <head>
		<title><asp:literal id="PageTitle" runat="server"></asp:literal></title>
		<link id="CssStyleSheet" rel="stylesheet" type="text/css" href="../Css/Default.css" runat="server" />
		<link id="ModuleCss" rel="stylesheet" type="text/css" href="../Css/Modules.css" runat="server" />
  </head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="container">
				<div id="header">
					Header
				</div>
				<div id="side1">
					Side 1
				</div>
				<div id="main">
					Main
					<asp:placeholder id="C_Main" runat="server"></asp:placeholder>
				</div>
			</div>
		</form>
	</body>
</html>
