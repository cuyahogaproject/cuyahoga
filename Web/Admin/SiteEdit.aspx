<%@ Page language="c#" Codebehind="SiteEdit.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.SiteEdit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>SiteEdit</title>
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
						<td style="WIDTH: 100px">Name</td>
						<td><asp:textbox id="txtName" runat="server" width="300px" maxlength="100"></asp:textbox><asp:requiredfieldvalidator id="rfvName" runat="server" enableclientscript="False" controltovalidate="txtName" cssclass="validator" display="Dynamic">Name is required</asp:requiredfieldvalidator></td></tr>
					<tr>
					<tr>
						<td>Site url (incl. http://)</td>
						<td><asp:textbox id="txtSiteUrl" runat="server" width="300px" maxlength="100"></asp:textbox><asp:requiredfieldvalidator id="rfvSiteUrl" runat="server" enableclientscript="False" controltovalidate="txtSiteUrl" cssclass="validator" display="Dynamic">Site url is required</asp:requiredfieldvalidator></td></tr></table></div>
			<div class="group">
				<h4>Defaults</h4>
				<table>
					<tr>
						<td style="WIDTH: 100px">Template</td>
						<td><asp:dropdownlist id="ddlTemplates" runat="server"></asp:dropdownlist></td></tr>
					<tr>
					<tr>
						<td>Culture</td>
						<td><asp:dropdownlist id="ddlCultures" runat="server"></asp:dropdownlist></td></tr></table></div>
			<div>
				<asp:button id="btnSave" runat="server" text="Save"></asp:button>
				<asp:button id="btnCancel" runat="server" text="Cancel" causesvalidation="False"></asp:button>
				<asp:button id="btnDelete" runat="server" text="Delete"></asp:button>
			</div></form>

	</body>
</html>
