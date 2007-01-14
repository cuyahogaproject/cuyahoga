<%@ Page language="c#" Codebehind="Install.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Install.Install" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Install</title>
		<link rel="stylesheet" href="../Admin/Css/Admin.css" type="text/css" />
	</head>
	<body>
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
				<asp:panel id="pnlModules" cssclass="group" runat="server" visible="false">
					<h4>Optional modules</h4>
					Choose the optional modules to install:
					<table class="tbl">
						<asp:repeater id="rptModules" runat="server">
							<itemtemplate>
							<tr>
								<td>
									<asp:checkbox id="chkInstall" runat="server" checked="true" />
								</td>
								<td>
									<asp:literal id="litModuleName" runat="server" text="<%# Container.DataItem %>" />
								</td>
							</tr>
							</itemtemplate>
						</asp:repeater>
					</table>
					<asp:button id="btnInstallModules" runat="server" text="Install selected modules" onclick="btnInstallModules_Click" />
					<asp:button id="btnSkipInstallModules" runat="server" text="Skip installing optional modules" onclick="btnSkipInstallModules_Click"  />
				</asp:panel>
				<asp:panel id="pnlAdmin" cssclass="group" runat="server" visible="False">
					<h4>Set administrator password</h4>The default administrator has the username 
					'admin'. Please enter a password for the administrator. 
					<table class="tbl">
						<tr>
							<td style="width:100px">Username</td>
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
