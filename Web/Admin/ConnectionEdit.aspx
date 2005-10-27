<%@ Page language="c#" Codebehind="ConnectionEdit.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.ConnectionEdit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>ConnectionEdit</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div class="group">
				<h4>From section</h4>
				<table>
					<tr>
						<td style="WIDTH:100px">Section</td>
						<td>
							<asp:label id="lblSectionFrom" runat="server"></asp:label></td>
					</tr>
					<tr>
						<td>Module type</td>
						<td>
							<asp:label id="lblModuleType" runat="server"></asp:label></td>
					</tr>
					<tr>
						<td>Action</td>
						<td>
							<asp:dropdownlist id="ddlAction" runat="server" autopostback="True"></asp:dropdownlist></td>
					</tr>
				</table>
			</div>
			<asp:panel id="pnlTo" cssclass="group" runat="server" visible="False">
				<h4>To section</h4>
				<table>
					<tr>
						<td style="WIDTH: 100px">Section</td>
						<td>
							<asp:dropdownlist id="ddlSectionTo" runat="server"></asp:dropdownlist></td>
					</tr>
				</table>
			</asp:panel>
			<div>
				<asp:button id="btnSave" runat="server" text="Save" enabled="False"></asp:button>
				<asp:button id="btnBack" runat="server" text="Back" causesvalidation="False"></asp:button>
			</div>
		</form>
	</body>
</html>
