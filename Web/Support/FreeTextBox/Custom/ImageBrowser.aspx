<%@ Page language="c#" Codebehind="ImageBrowser.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Support.FreeTextBox.Custom.ImageBrowser" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>ImageBrowser</title>
		<link href="SupportCustom.css" type="text/css" rel="stylesheet">
			<script language="javascript">
			function selectImage() {
				if (document.getElementById("txtUrl").value != "") {
					imageLink = "<img src=\"" + document.getElementById("txtUrl").value + "\"";
					if (document.getElementById("txtWidth").value != "") {
						imageLink += " width=\"" + document.getElementById("txtWidth").value + "\"";
					}
					if (document.getElementById("txtHeight").value != "") {
						imageLink += " height=\"" + document.getElementById("txtHeight").value + "\"";
					}
					imageLink += " alt=\"" + document.getElementById("txtAlt").value + "\"";
					imageLink += " />";
					window.opener.FTB_InsertText("<% Response.Write(Request.QueryString["textboxname"]); %>", imageLink);
					
					this.close();	
				}
			}
			</script>
	</head>
	<body>
		<form id="Form1" method="post" enctype="multipart/form-data" runat="server">
			<div id="header"><h1>Insert image</h1></div>
			<div id="folder">Current Folder: <asp:label id="lblFolder" runat="server" font-bold="True">/</asp:label></div>
			<!-- <asp:linkbutton id=lbtNewFolder runat="server">Create new folder</asp:linkbutton>&nbsp;<asp:linkbutton id=lbtDeleteFolder runat="server">Delete current folder</asp:linkbutton></div> -->
			<div id="container">
				<div id="browser">
					<ul id="fileList"><asp:repeater id="rptItems" runat="server">
							<itemtemplate>
								<li><asp:image id="imgIcon" runat="server" borderwidth="0"></asp:image>
									<asp:linkbutton id="lbtItem" runat="server"><%# DataBinder.Eval(Container.DataItem, "Name") %></asp:linkbutton></li>
							</itemtemplate>
						</asp:repeater></ul></div>
				<table border="0">
					<tr>
						<td width="54">Preview</td>
						<td><asp:image id="imgPreview" runat="server" visible="False"></asp:image></td></tr>
					<tr>
						<td width="54" height="18">Width</td>
						<td height="22"><asp:textbox id="txtWidth" runat="server" width="30px"></asp:textbox></td></tr>
					<tr>
						<td width="54">Height</td>
						<td><asp:textbox id="txtHeight" runat="server" width="30px"></asp:textbox></td></tr>
					<tr>
						<td width="54">Alt text</td>
						<td><asp:textbox id="txtAlt" runat="server"></asp:textbox></td></tr></table>
				<div id="controls">
					<table border="0">
						<tr>
							<td>Image url</td>
							<td><asp:textbox id="txtUrl" runat="server" style="width:320px;"></asp:textbox></td></tr>
						<tr>
							<td>Upload image</td>
							<td><input id="uplImage" style="WIDTH: 320px" type="file" runat="server"> <asp:button id="btnUpload" runat="server" text="Upload"></asp:button></td></tr></table></div></div>
			<div id="dialogButtons"><input id="btnSelect" onclick="selectImage();" type="button" value="OK">
				<input id="btnCancel" onclick="window.close();" type="button" value="Cancel">
			</div></form>
	</body>
</html>
