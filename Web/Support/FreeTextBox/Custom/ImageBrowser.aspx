<%@ Page language="c#" Codebehind="ImageBrowser.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Support.FreeTextBox.Custom.ImageBrowser" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
  <head>
		<title>ImageBrowser</title>
<LINK href="SupportCustom.css" type=text/css rel=stylesheet >
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
			imageLink += ">";
			alert(imageLink);
			window.opener.FTB_InsertText("<% Response.Write(Request.QueryString["textboxname"]); %>", imageLink);
			
			this.close();	
		}
	}
	</script>
  </head>
<body>
<form id=Form1 method=post runat="server">
<div id=container>
<div>Current Folder: <asp:label id=lblFolder runat="server">/</asp:label></DIV>
<div id=filebrowser>
<ul id=fileList><asp:repeater id=rptItems runat="server">
							<itemtemplate>
								<li><asp:image id="imgIcon" runat="server" borderwidth="0"></asp:image>
									<asp:linkbutton id="lbtItem" runat="server"><%# DataBinder.Eval(Container.DataItem, "Name") %></asp:linkbutton></li>
							</itemtemplate>
						</asp:repeater></UL></DIV>
<table border=0>
  <tr>
    <td>Preview</TD>
    <td><iframe id=imgpreview marginWidth=0 
      marginHeight=0 src="Thumbnail.aspx" frameBorder=0 
      runat="server"></IFRAME></TD></TR>
  <tr>
    <td>Width</TD>
							<td><asp:textbox id=txtWidth runat="server" width="30px"></asp:textbox></td></TR>
						<tr>
							<td>Height</td>
												<td><asp:textbox id="txtHeight" runat="server" width="30px"></asp:textbox></td></tr>
						<tr>
							<td>Alt text</td>
							<td><asp:TextBox id=txtAlt runat="server"></asp:TextBox></td></tr></TABLE>
				<div id="controls">
					<div>
						Image url
						<asp:textbox id="txtUrl" runat="server" width="350px"></asp:textbox><input id="btnSelect" onclick="selectImage();" type="button" value="Select Image">
					</div>
					<div>
						 Upload&nbsp;image <input type="file"><asp:button id="btnUpload" runat="server" text="Upload"></asp:button>

					</div>
				</div></DIV></FORM>

	</body>
</html>
