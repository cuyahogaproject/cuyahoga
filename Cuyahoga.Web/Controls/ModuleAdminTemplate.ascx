<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ModuleAdminTemplate.ascx.cs" Inherits="Cuyahoga.Web.Controls.ModuleAdminTemplate" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="header" Src="../Controls/ModuleAdminHeader.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 TRANSITIONAL//EN" >
<html>
	<head>
		<title><asp:literal id="PageTitle" runat="server"></asp:literal></title>
		<link id="CssStyleSheet" rel="stylesheet" type="text/css" runat="server" />
		<script type="text/javascript" src="<%= ResolveUrl("~/Support/jquery/jquery-1.3.2.min.js") %>"></script>
	</head>
	<body>
		<form id="Frm" method="post" enctype="multipart/form-data" runat="server">
			<uc1:header id="header" runat="server"></uc1:header>
			<div id="MessageBox" class="messagebox" runat="server" visible="false" enableviewstate="false"></div>
			<asp:placeholder id="PageContent" runat="server"></asp:placeholder>
			<script type="text/javascript">
				$(document).ready(function() {
					// Hide module admin header when we're inside an iframe
					var isInsideIframe = (window.location != window.parent.location) ? true : false;
					if (isInsideIframe) {
						$('#header').hide();
						$('#subheader').hide();
					}
				});
			</script>
		</form>
	</body>
</html>