<%@ Register TagPrefix="cc1" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<%@ Page language="c#" Codebehind="EditFile.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Downloads.Web.EditFile" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>EditFile</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Edit File</h1>
				<div class="group">
					<h4>File properties</h4>
					<table>
						<tr>
							<td style="WIDTH: 100px">File</td>
							<td><asp:panel id="pnlFileName" runat="server" visible="True">
									<asp:textbox id="txtFile" runat="server" width="300px" enabled="False"></asp:textbox>
									<asp:requiredfieldvalidator id="rfvFile" runat="server" errormessage="File is required" display="Dynamic" cssclass="validator"
										controltovalidate="txtFile" enableclientscript="False"></asp:requiredfieldvalidator>
								</asp:panel><input id="filUpload" style="WIDTH: 300px" type="file" runat="server">
								<asp:button id="btnUpload" runat="server" causesvalidation="False" text="Upload"></asp:button></td>
						</tr>
						<tr>
							<td style="WIDTH: 100px">Title (optional)</td>
							<td>
								<asp:textbox id="txtTitle" runat="server" width="300px"></asp:textbox></td>
						</tr>
						<tr>
							<td style="WIDTH: 100px">Date published</td>
							<td>
								<cc1:calendar id="calDatePublished" runat="server" displaytime="True"></cc1:calendar>
								<asp:requiredfieldvalidator id="rfvDatePublished" runat="server" errormessage="Date published is required" display="Dynamic"
									cssclass="validator" controltovalidate="calDatePublished" enableclientscript="False"></asp:requiredfieldvalidator></td>
						</tr>
					</table>
				</div>
				<div class="group">
					<h4>Allowed roles for download</h4>
					<table class="tbl">
						<asp:repeater id="rptRoles" runat="server">
							<headertemplate>
								<tr>
									<th>
										Role</th>
									<th>
									</th>
								</tr>
							</headertemplate>
							<itemtemplate>
								<tr>
									<td><%# DataBinder.Eval(Container.DataItem, "Role.Name") %></td>
									<td style="text-align:center">
										<asp:checkbox id="chkRole" runat="server"></asp:checkbox></td>
								</tr>
							</itemtemplate>
						</asp:repeater></table>
				</div>
				<p><asp:button id="btnSave" runat="server" text="Save"></asp:button><asp:button id="btnDelete" runat="server" visible="False" causesvalidation="False" text="Delete"></asp:button><asp:button id="btnCancel" runat="server" causesvalidation="False" text="Cancel"></asp:button></p>
			</div>
		</form>
	</body>
</html>
