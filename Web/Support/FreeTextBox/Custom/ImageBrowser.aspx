<%@ Page language="c#" Codebehind="ImageBrowser.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Support.FreeTextBox.Custom.ImageBrowser" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
  <head>
		<title>ImageBrowser</title>
<LINK href="SupportCustom.css" type=text/css rel=stylesheet >
<script language=javascript>
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
					imageLink += ">";
					window.opener.FTB_InsertText("<% Response.Write(Request.QueryString["textboxname"]); %>", imageLink);
					
					this.close();	
				}
			}
			</script>
</head>
<body>
<form id=Form1 method=post EncType="Multipart/Form-Data" runat="server">
<div id=header></DIV>
<div id=folder>Current Folder: <asp:label id=lblFolder runat="server" font-bold="True">/</asp:label></DIV>
			<!-- <asp:linkbutton id=lbtNewFolder runat="server">Create new folder</asp:linkbutton>&nbsp;<asp:linkbutton id=lbtDeleteFolder runat="server">Delete current folder</asp:linkbutton></div> -->
<div id=container>
<div id=filebrowser>
<ul id=fileList><asp:repeater id=rptItems runat="server">
							<itemtemplate>
								<li><asp:image id="imgIcon" runat="server" borderwidth="0"></asp:image>
									<asp:linkbutton id="lbtItem" runat="server"><%# DataBinder.Eval(Container.DataItem, "Name") %></asp:linkbutton></li>
							</itemtemplate>
						</asp:repeater></UL></DIV>
<table border=0>
  <tr>
    <td width=54>Preview</TD>
    <td><asp:image id=imgPreview runat="server" visible="False"></asp:image></TD></TR>
  <tr>
    <td width=54 height=18>Width</TD>
    <td height=22><asp:textbox id=txtWidth runat="server" width="30px"></asp:textbox></TD></TR>
  <tr>
    <td width=54>Height</TD>
    <td><asp:textbox id=txtHeight runat="server" width="30px"></asp:textbox></TD></TR>
  <tr>
    <td width=54>Alt text</TD>
    <td><asp:textbox id=txtAlt runat="server"></asp:textbox></TD></TR></TABLE>
<div id=controls>
<table border=0>
  <tr>
    <td>Image url</TD>
    <td><asp:textbox id=txtUrl runat="server" width="320px"></asp:textbox></TD></TR>
  <tr>
    <td>Upload image</TD>
    <td><input id=uplImage style="WIDTH: 320px" type=file 
       runat="server"> <asp:button id=btnUpload runat="server" text="Upload"></asp:button></TD></TR></TABLE></DIV></DIV>
<div id=dialogButtons><input id=btnSelect onclick=selectImage(); type=button value=OK> 
<input id=btnCancel onclick=window.close(); type=button value=Cancel> 
</DIV></FORM>
	</body>
</html>
