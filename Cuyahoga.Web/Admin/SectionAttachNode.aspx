<%@ Page language="c#" Codebehind="SectionAttachNode.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.SectionAttachNode" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>SectionAttach</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div class="group">
				<h4>Section</h4>
				<table>
					<tr>
						<td style="WIDTH:100px">Section</td>
						<td>
							<asp:label id="lblSection" runat="server"></asp:label></td>
					</tr>
					<tr>
						<td>Module type</td>
						<td>
							<asp:label id="lblModuleType" runat="server"></asp:label></td>
					</tr>
				</table>
			</div>
			<div class="group">
				<h4>Attach to</h4>
				<table>
					<tr>
						<td style="WIDTH:100px">Site</td>
						<td><asp:dropdownlist id="ddlSites" runat="server" autopostback="true"></asp:dropdownlist></td>
					</tr>
					<tr>
						<td>Node</td>
						<td><asp:listbox id="lbxAvailableNodes" runat="server" height="200px" width="150px" autopostback="true"
								visible="False"></asp:listbox></td>
					</tr>
					<tr>
						<td>Placeholder</td>
						<td><asp:dropdownlist id="ddlPlaceholder" runat="server" visible="False"></asp:dropdownlist>
							<asp:hyperlink id="hplLookup" runat="server" visible="False">Lookup</asp:hyperlink></td>
					</tr>
				</table>
			</div>
			<div>
				<asp:button id="btnSave" runat="server" text="Save" enabled="False"></asp:button>
				<asp:button id="btnBack" runat="server" text="Back" causesvalidation="False"></asp:button>
			</div>
			<script type="text/javascript"> <!--
			function setPlaceholderValue(ddlist, val)
			{
				var placeholdersList = document.getElementById(ddlist);
				if (placeholdersList != null)
				{
					for (i = 0; i < placeholdersList.options.length; i++)
					{
						if (placeholdersList.options[i].value == val)
						{
							placeholdersList.selectedIndex = i;
						}
					}				
				}
			}
			// -->
			</script>
		</form>
	</body>
</html>
