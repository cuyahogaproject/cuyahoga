<%@ Page language="c#" Codebehind="LinkBrowser.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Support.FreeTextBox.Custom.LinkBrowser" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>LinkBrowser</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"><link href="SupportCustom.css" type="text/css" rel="stylesheet">
		<script language="javascript">
		function selectLink() {
			var link = "";
			if (document.getElementById("txtUrl").value != "" || document.getElementById("txtDescription").value != "") {
				link += "<a href=\"" + document.getElementById("txtUrl").value + "\"";
				link += " target=\"" + document.getElementById("ddlTarget").value + "\"";
				link += ">";
				link += document.getElementById("txtDescription").value;
				link += "</a>";
				window.opener.FTB_InsertText("<% Response.Write(Request.QueryString["textboxname"]); %>", link);
				
				this.close();	
			}
			else {
				alert("The url and the description are required");
			}
		}
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="header"><h1>Create hyperlink</h1></div>
			<div id="folder"></div>
			<div id="container">
				<div id="browser">
					<asp:placeholder id="plhNodes" runat="server"></asp:placeholder>
				</div>
				<div>Select an item from the list or enter the complete url
					in the box below. </div>
				<div id="controls">
					<table border="0">
						<tr>
							<td style="WIDTH: 54px">Url</td>
							<td><asp:textbox id="txtUrl" runat="server" width="320"></asp:textbox></td></tr>
						<tr>
							<td>Link text</td>
							<td><asp:textbox id="txtDescription" runat="server" width="320"></asp:textbox></td></tr>
						<tr>
							<td style="WIDTH: 54px">Target</td>
							<td><asp:dropdownlist id="ddlTarget" runat="server">
									<asp:listitem value="_self" selected="True">Same window</asp:listitem>
									<asp:listitem value="_blank">New window</asp:listitem>
									<asp:listitem value="_top">Top window</asp:listitem>
									<asp:listitem value="_parent">Parent window</asp:listitem></asp:dropdownlist></td></tr></table></div></div>
			<div id="dialogButtons"><input id="btnSelect" onclick="selectLink();" type="button" value="OK">
				<input id="btnCancel" onclick="window.close();" type="button" value="Cancel">
			</div></form>
	</body>
</html>
