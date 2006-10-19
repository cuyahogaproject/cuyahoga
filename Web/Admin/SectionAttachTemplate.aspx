<%@ Page language="c#" Codebehind="SectionAttachTemplate.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.SectionAttachTemplate" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>SectionAttachTemplate</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div class="group">
				<h4>Section</h4>
				<table>
					<tr>
						<td style="WIDTH: 100px">Section</td>
						<td><asp:label id="lblSection" runat="server"></asp:label></td>
					</tr>
					<tr>
						<td>Module type</td>
						<td><asp:label id="lblModuleType" runat="server"></asp:label></td>
					</tr>
				</table>
			</div>
			<div class="group">
				<h4>Attach to</h4>
				<table class="tbl">
					<tr>
						<th>
							Template</th>
						<th>
							Attached</th>
						<th>
							Placeholder</th></tr>
					<asp:repeater id="rptTemplates" runat="server">
						<itemtemplate>
							<tr>
								<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
								<td style="text-align:center">
									<asp:checkbox id="chkAttached" runat="server"></asp:checkbox></td>
								<td>
									<asp:dropdownlist id="ddlPlaceHolders" runat="server"></asp:dropdownlist>
									<asp:hyperlink id="hplLookup" runat="server">Lookup</asp:hyperlink>	
								</td>
							</tr>
						</itemtemplate>
					</asp:repeater></table>
			</div>
			<div>
				<asp:button id="btnSave" runat="server" text="Save"></asp:button>
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
