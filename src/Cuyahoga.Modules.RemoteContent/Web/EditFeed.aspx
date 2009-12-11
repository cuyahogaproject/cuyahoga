<%@ Page language="c#" Codebehind="EditFeed.aspx.cs" AutoEventWireup="True" Inherits="Cuyahoga.Modules.RemoteContent.Web.EditFeed" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>EditFeed</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Edit Feed</h1>
				<div class="group">
					<h4>Feed properties</h4>
					<table>
						<tr>
							<td style="WIDTH: 100px">Url (incl http://)</td>
							<td>
								<asp:textbox id="txtUrl" runat="server" width="400px"></asp:textbox>
								<asp:button id="btnVerify" runat="server" text="Verify feed" causesvalidation="False" onclick="btnVerify_Click"></asp:button>
								<asp:requiredfieldvalidator id="rfvUrl" runat="server" errormessage="Url is required" display="Dynamic" cssclass="validator"
									enableclientscript="False" controltovalidate="txtUrl"></asp:requiredfieldvalidator></td>
						</tr>
						<tr>
							<td>Title</td>
							<td>
								<asp:textbox id="txtTitle" runat="server" width="400px"></asp:textbox>
								<asp:requiredfieldvalidator id="rfvTitle" runat="server" errormessage="Title is required" display="Dynamic"
									cssclass="validator" enableclientscript="False" controltovalidate="txtTitle"></asp:requiredfieldvalidator></td>
						</tr>
						<tr>
							<td>Number of items to show</td>
							<td>
								<asp:textbox id="txtNumberOfItems" runat="server" width="40px"></asp:textbox>
								<asp:requiredfieldvalidator id="rfvNumberOfItems" runat="server" errormessage="Number of items is required"
									display="Dynamic" cssclass="validator" enableclientscript="False" controltovalidate="txtNumberOfItems"></asp:requiredfieldvalidator>
								<asp:comparevalidator id="covNumberOfItems" runat="server" errormessage="Invalid number" display="Dynamic"
									cssclass="validator" enableclientscript="False" controltovalidate="txtNumberOfItems" operator="DataTypeCheck"
									type="Integer"></asp:comparevalidator></td>
						</tr>
						<tr>
							<td>Feed publication date</td>
							<td>
								<asp:label id="lblPubDate" runat="server"></asp:label></td>
						</tr>
						<tr>
							<td>Last updated</td>
							<td>
								<asp:label id="lblUpdateTimestamp" runat="server"></asp:label></td>
						</tr>
					</table>
				</div>
				<p>
					<asp:button id="btnSave" runat="server" text="Save" onclick="btnSave_Click"></asp:button>
					<asp:button id="btnDelete" runat="server" text="Delete" visible="False" causesvalidation="False" onclick="btnDelete_Click"></asp:button>
					<input id="btnCancel" type="button" value="Cancel" runat="server" name="btnCancel">
				</p>
			</div>
		</form>
	</body>
</html>
