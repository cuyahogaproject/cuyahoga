<%@ Page language="c#" Codebehind="Install.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Install.Install" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Install</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" href="../Admin/Css/Admin.css" type="text/css">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Cuyahoga Installation</h1>
				<asp:panel id="pnlErrors" cssclass="errorbox" visible="False" runat="server">
					<asp:label id="lblError" runat="server"></asp:label>
				</asp:panel>
				<asp:panel id="pnlMessage" cssclass="messagebox" visible="False" runat="server">
					<asp:label id="lblMessage" runat="server"></asp:label>
				</asp:panel>
				<asp:panel id="pnlIntro" cssclass="group" runat="server" visible="False">
<h4>Create Database</h4>The installer will first install the database. After 
that you have to set the password for the default administrator. <br/><br/>The 
database for the following components wil be installed: <br/>
<asp:label id="lblCoreAssembly" runat="server" font-bold="True"></asp:label><br/>
<asp:label id="lblModulesAssembly" runat="server" font-bold="True"></asp:label><br/><br/>
<asp:button id="btnInstallDatabase" runat="server" text="Install database"></asp:button>
				</asp:panel>
				<asp:panel id="pnlAdmin" cssclass="group" runat="server" visible="False">
<h4>Set administrator password</h4>The default administrator has the username 
'admin'. Please enter a password for the administrator. 
<table class="tbl">
						<tr>
							<td width="100">Username</td>
							<td>admin</td>
						</tr>
						<tr>
							<td>Password</td>
							<td>
								<asp:textbox id="txtPassword" runat="server" width="100px" textmode="Password"></asp:textbox>
								<asp:requiredfieldvalidator id="rfvPassword" runat="server" cssclass="validator" enableclientscript="False"
									controltovalidate="txtPassword" display="Dynamic" errormessage="The password is required"></asp:requiredfieldvalidator></td>
						</tr>
						<tr>
							<td>Confirm password</td>
							<td>
								<asp:textbox id="txtConfirmPassword" runat="server" width="100px" textmode="Password"></asp:textbox>
								<asp:requiredfieldvalidator id="rfvConfirmPassword" runat="server" cssclass="validator" enableclientscript="False"
									controltovalidate="txtConfirmPassword" display="Dynamic" errormessage="The password is required"></asp:requiredfieldvalidator>
								<asp:comparevalidator id="cpvPassword" runat="server" enableclientscript="False" controltovalidate="txtConfirmPassword"
									errormessage="The passwords must be the same" controltocompare="txtPassword"></asp:comparevalidator></td>
						</tr>
					</table><br/>
<asp:button id="btnAdmin" runat="server" text="Create administrator account"></asp:button>		
				</asp:panel>
				<asp:panel id="pnlCreateSite" cssclass="group" runat="server" visible="False">
<h4>Create site</h4>Do you want Cuyahoga to create a basic site for you? 
<br/><br/>
<asp:button id="btnCreateSite" runat="server" text="Yes, create a site"></asp:button>
<asp:button id="btnSkipCreateSite" runat="server" text="No, skip this step"></asp:button>
					
					
				</asp:panel>
				<asp:panel id="pnlFinished" cssclass="group" runat="server" visible="False">
<h4>Finished</h4>The Cuyahoga is successfully installed! <br/><br/>
<asp:hyperlink id="hplContinue" runat="server" navigateurl="~/Admin">Log in
					to the site administration with the account you just created to create a site and continue.</asp:hyperlink>
				</asp:panel>
			</div>
		</form>
	</body>
</html>
