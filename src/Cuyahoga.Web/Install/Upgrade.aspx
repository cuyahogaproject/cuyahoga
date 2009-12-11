<%@ Page language="c#" Codebehind="Upgrade.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Install.Upgrade" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Upgrade</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" href="../Admin/Css/Admin.css" type="text/css">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Cuyahoga Upgrade</h1>
				<asp:panel id="pnlErrors" cssclass="errorbox" visible="False" runat="server">
					<asp:label id="lblError" runat="server"></asp:label>
				</asp:panel>
				<asp:panel id="pnlIntro" cssclass="group" runat="server" visible="False">
<h4>Upgrade Database</h4>Current versions:<br/>
<asp:label id="lblCoreAssemblyCurrent" runat="server" font-bold="True"></asp:label><br/>
<asp:label id="lblModulesAssemblyCurrent" runat="server" font-bold="True"></asp:label><br/><br/>New 
versions:<br/>
<asp:label id="lblCoreAssemblyNew" runat="server" font-bold="True"></asp:label><br/>
<asp:label id="lblModulesAssemblyNew" runat="server" font-bold="True"></asp:label><br/><br/>
<asp:button id="btnUpgradeDatabase" runat="server" text="Upgrade database"></asp:button>
				</asp:panel>
				<asp:panel id="pnlFinished" cssclass="group" runat="server" visible="False">
<h4>Finished</h4>Cuyahoga is upgraded successfully!<br />
It's highly recommended to visit the module administration page to check if there are updates for existing modules.<br/><br/>
<asp:hyperlink id="hplModules" runat="server" navigateurl="~/Admin/Modules.aspx">Upgrade modules or check module status</asp:hyperlink><br/>
<asp:hyperlink id="hplSite" runat="server" navigateurl="~/Default.aspx">View the site</asp:hyperlink><br/>
<asp:hyperlink id="hplAdmin" runat="server" navigateurl="~/Admin">Go to
					to the site administration</asp:hyperlink>
				</asp:panel>
			</div>
		</form>
	</body>
</html>
