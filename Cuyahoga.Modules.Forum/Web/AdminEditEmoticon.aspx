<%@ Register TagPrefix="cc1" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<%@ Page language="c#" Codebehind="AdminEditEmoticon.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Forum.AdminEditEmoticon" ValidateRequest="false" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Edit Emoticon</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Edit forum emoticon</h1>
				<div class="group">
					<h4>Emoticon</h4>
					<table>
						<tr>
							<td style="WIDTH: 100px">Text version</td>
							<td><asp:textbox id="txtTextVersion" runat="server" width="592px"></asp:textbox><asp:requiredfieldvalidator id="rfvTextVersion" runat="server" cssclass="validator" display="Dynamic" errormessage="Text version is required"
									enableclientscript="False" controltovalidate="txtTextVersion"></asp:requiredfieldvalidator></td>
						</tr>
						<TR>
							<TD style="WIDTH: 100px">Image name</TD>
							<TD>
								<asp:textbox id="txtImageName" runat="server" width="592px"></asp:textbox>
								<asp:requiredfieldvalidator id="rfvImageName" runat="server" controltovalidate="txtImageName" enableclientscript="False"
									errormessage="Image name is required" display="Dynamic" cssclass="validator"></asp:requiredfieldvalidator></TD>
						</TR>
					</table>
				</div>
				<p><asp:button id="btnSave" runat="server" text="Save"></asp:button><asp:button id="btnDelete" runat="server" text="Delete" visible="False"></asp:button><input id="btnCancel" type="button" value="Cancel" runat="server"></p>
			</div>
		</form>
	</body>
</HTML>
