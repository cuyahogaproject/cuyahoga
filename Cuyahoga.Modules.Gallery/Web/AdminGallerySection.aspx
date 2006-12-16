<%@ Page language="c#" Codebehind="AdminGallerySection.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Gallery.Web.AdminGallerySection" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AdminGallerySection</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Gallery List and PhotoShow management</h1>
				<p>Select the Gallery you want to link to this section in the list below or press 
					the new Gallery button if you want to create a new Gallery</p>
				<asp:RadioButtonList id="rblGalleries" runat="server" AutoPostBack="True"></asp:RadioButtonList>
				<p><INPUT id="btnNew" type="button" value="New Gallery" name="btnNew" runat="server">
				</p>
			</div>
		</form>
	</body>
</HTML>
