<%@ Page language="c#" Codebehind="MenuEdit.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.MenuEdit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 TRANSITIONAL//EN" >
<html>
	<head>
		<title>MenuEdit</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div class="group">
				<h4>General</h4>
				<table>
					<tr>
						<td style="WIDTH: 200px">Name</td>
						<td><asp:textbox id="txtName" runat="server" width="200px"></asp:textbox>
							<asp:requiredfieldvalidator id="rfvName" runat="server" errormessage="Name is required" cssclass="validator"
								display="Dynamic" controltovalidate="txtName" enableclientscript="False" enableviewstate="False"></asp:requiredfieldvalidator></td>
					</tr>
					<tr>
						<td>Placeholder</td>
						<td>
							<asp:dropdownlist id="ddlPlaceholder" runat="server"></asp:dropdownlist></td>
					</tr>
				</table>
			</div>
			<div class="group">
				<h4>Nodes</h4>
			</div>
			<div><asp:button id="btnSave" runat="server" text="Save"></asp:button><asp:button id="btnBack" runat="server" text="Back" causesvalidation="False"></asp:button><asp:button id="btnDelete" runat="server" text="Delete" causesvalidation="False"></asp:button></div>
		</form>
	</body>
</html>
