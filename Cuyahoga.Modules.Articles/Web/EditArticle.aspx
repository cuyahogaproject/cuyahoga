<%@ Page language="c#" Codebehind="EditArticle.aspx.cs" AutoEventWireup="True" Inherits="Cuyahoga.Modules.Articles.Web.EditArticle" ValidateRequest="false" %>
<%@ Register TagPrefix="cc1" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>EditArticle</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Edit Article</h1>
				<div class="group">
					<h4>Article</h4>
					<table>
						<tr>
							<td style="WIDTH: 100px">Title</td>
							<td><asp:textbox id="txtTitle" runat="server" width="650px"></asp:textbox><asp:requiredfieldvalidator id="rfvTitle" runat="server" cssclass="validator" display="Dynamic" errormessage="The title is required"
									enableclientscript="False" controltovalidate="txtTitle"></asp:requiredfieldvalidator></td>
						</tr>
						<tr>
							<td style="WIDTH: 100px">Summary</td>
							<td><asp:textbox id="txtSummary" runat="server" width="650px" height="60px" textmode="MultiLine"></asp:textbox></td>
						</tr>
						<tr>
							<td style="WIDTH: 100px">Content</td>
							<td>
								<fckeditorv2:fckeditor id="fckContent" runat="server" height="350px" width="650px"></fckeditorv2:fckeditor></td>
						</tr>
						<tr>
							<td style="WIDTH: 100px">Category</td>
							<td><asp:dropdownlist id="ddlCategory" runat="server" width="200px"></asp:dropdownlist>&nbsp;or 
								enter a new category:
								<asp:textbox id="txtCategory" runat="server" width="200px"></asp:textbox>
							</td>
						</tr>
					</table>
				</div>
				<div class="group">
					<h4>Publishing</h4>
					<table>
						<tr>
							<td style="WIDTH: 100px">Syndicate</td>
							<td><asp:checkbox id="chkSyndicate" runat="server" checked="True"></asp:checkbox></td>
						</tr>
						<tr>
							<td style="WIDTH: 100px">Date online</td>
							<td><cc1:calendar id="calDateOnline" runat="server" displaytime="True"></cc1:calendar></td>
						</tr>
						<tr>
							<td style="WIDTH: 100px">Date offline</td>
							<td><cc1:calendar id="calDateOffline" runat="server" displaytime="True"></cc1:calendar></td>
						</tr>
					</table>
				</div>
				<p><asp:button id="btnSave" runat="server" text="Save" onclick="btnSave_Click"></asp:button><asp:button id="btnDelete" runat="server" text="Delete" visible="False" onclick="btnDelete_Click"></asp:button><input id="btnCancel" type="button" value="Cancel" runat="server"></p>
			</div>
		</form>
	</body>
</html>
