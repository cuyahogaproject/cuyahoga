<%@ Page language="c#" Codebehind="NodeEdit.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.NodeEdit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 TRANSITIONAL//EN" >
<html>
	<head>
		<title>Cuyahoga Site Administration</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Css/Admin.css" type="text/css" rel="stylesheet">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<script language="javascript"> <!--
				function confirmDeleteNode()
				{
					return confirm("Are you sure you want to delete this node?");
				}
				// -->
			</script>
			<div class="group">
				<h4>General</h4>
				<table>
					<tr>
						<td style="WIDTH: 100px">Node title</td>
						<td><asp:textbox id="txtTitle" runat="server" width="300px"></asp:textbox><asp:requiredfieldvalidator id="rfvTitle" runat="server" enableclientscript="False" controltovalidate="txtTitle"
								cssclass="validator" display="Dynamic" errormessage="Title is required"></asp:requiredfieldvalidator></td>
					</tr>
					<tr>
						<td>Friendly url</td>
						<td><asp:textbox id="txtShortDescription" runat="server" width="300px" tooltip="You can use this for 'nice' links ([shortdescription].aspx). Make sure it's unique!"></asp:textbox>.aspx&nbsp;<asp:regularexpressionvalidator id="revShortDescription" runat="server" enableclientscript="False" controltovalidate="txtShortDescription"
								cssclass="validator" display="Dynamic" errormessage="No spaces are allowed" validationexpression="\S+"></asp:regularexpressionvalidator><asp:requiredfieldvalidator id="rfvShortDescription" runat="server" errormessage="Short description is required"
								controltovalidate="txtShortDescription" display="Dynamic" enableclientscript="False"></asp:requiredfieldvalidator></td>
					</tr>
					<tr>
						<td>Parent node</td>
						<td><asp:label id="lblParentNode" runat="server"></asp:label></td>
					</tr>
					<tr>
						<td>Position</td>
						<td><asp:imagebutton id="btnUp" runat="server" alternatetext="Move up" causesvalidation="False" imageurl="../Images/upred.gif"></asp:imagebutton><asp:imagebutton id="btnDown" runat="server" alternatetext="Move down" causesvalidation="False" imageurl="../Images/downred.gif"></asp:imagebutton><asp:imagebutton id="btnLeft" runat="server" alternatetext="Move left" causesvalidation="False" imageurl="../Images/leftred.gif"></asp:imagebutton><asp:imagebutton id="btnRight" runat="server" alternatetext="Move right" causesvalidation="False"
								imageurl="../Images/rightred.gif"></asp:imagebutton></td>
					</tr>
					<tr>
						<td>Culture</td>
						<td><asp:dropdownlist id="ddlCultures" runat="server"></asp:dropdownlist></td>
					</tr>
					<tr>
						<td>Show in navigation</td>
						<td><asp:checkbox id="chkShowInNavigation" runat="server" checked="True"></asp:checkbox></td>
					</tr>
				</table>
			</div>
			<div class="group">
				<h4>Template</h4>
				<table>
					<tr>
						<td style="WIDTH: 100px">&nbsp;</td>
						<td><asp:dropdownlist id="ddlTemplates" runat="server" autopostback="True"></asp:dropdownlist></td>
					</tr>
				</table>
			</div>
			<asp:panel id="pnlMenus" cssclass="group" runat="server" visible="False">
				<h4>Menus</h4>
				<em>You're editing a root node, so you can also attach on or more custom menu's.</em>
				<table class="tbl">
					<asp:repeater id="rptMenus" runat="server">
						<headertemplate>
							<tr>
								<th>
									Menu</th>
								<th>
									Placeholder</th>
								<th>
									Actions</th>
							</tr>
						</headertemplate>
						<itemtemplate>
							<tr>
								<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
								<td><%# DataBinder.Eval(Container.DataItem, "Placeholder") %></td>
								<td>
									<asp:hyperlink id="hplEditMenu" runat="server">Edit</asp:hyperlink>
								</td>
							</tr>
						</itemtemplate>
					</asp:repeater></table>
				<asp:hyperlink id="hplNewMenu" runat="server">Add menu</asp:hyperlink>
			</asp:panel>
			<div class="group">
				<h4>Sections</h4>
				<table class="tbl">
					<asp:repeater id="rptSections" runat="server">
						<headertemplate>
							<tr>
								<th>
									Section title</th>
								<th>
									Module type</th>
								<th>
									Placeholder</th>
								<th>
									Cache duration</th>
								<th>
									Position</th>
								<th>
									Actions</th>
							</tr>
						</headertemplate>
						<itemtemplate>
							<tr>
								<td><%# DataBinder.Eval(Container.DataItem, "Title") %></td>
								<td><%# DataBinder.Eval(Container.DataItem, "ModuleType.Name") %></td>
								<td><%# DataBinder.Eval(Container.DataItem, "PlaceholderId") %>
									<asp:label id="lblNotFound" cssclass="validator" visible="False" runat="server">(not found in template!)</asp:label></td>
								<td style="text-align:right"><%# DataBinder.Eval(Container.DataItem, "CacheDuration") %></td>
								<td>
									<asp:hyperlink id="hplSectionUp" imageurl="../Images/upred.gif" visible="False" enableviewstate="False"
										runat="server">Move up</asp:hyperlink>
									<asp:hyperlink id="hplSectionDown" imageurl="../Images/downred.gif" visible="False" enableviewstate="False"
										runat="server">Move down</asp:hyperlink>
								</td>
								<td>
									<asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink>
									<asp:hyperlink id="hplDetach" runat="server">Detach</asp:hyperlink>
									<asp:linkbutton id="lbtDelete" runat="server" causesvalidation="False" commandname="Delete" commandargument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>Delete</asp:linkbutton>
								</td>
							</tr>
						</itemtemplate>
					</asp:repeater></table>
				<asp:hyperlink id="hplNewSection" runat="server" visible="False">Add section</asp:hyperlink></div>
			<div class="group">
				<h4>Authorization</h4>
				<table class="tbl">
					<asp:repeater id="rptRoles" runat="server">
						<headertemplate>
							<tr>
								<th>
									Role</th>
								<th>
									View allowed</th>
								<th>
									Edit allowed</th>
							</tr>
						</headertemplate>
						<itemtemplate>
							<tr>
								<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
								<td style="text-align:center">
									<asp:checkbox id="chkViewAllowed" runat="server"></asp:checkbox></td>
								<td style="text-align:center">
									<asp:checkbox id="chkEditAllowed" runat="server"></asp:checkbox></td>
							</tr>
						</itemtemplate>
					</asp:repeater>
				</table>
				<asp:checkbox id="chkPropagateToSections" runat="server" text="Propagate security settings to sections"></asp:checkbox>
				<br>
				<asp:checkbox id="chkPropagateToChildNodes" runat="server" text="Propagate security settings to child nodes"></asp:checkbox>
			</div>
			<div><asp:button id="btnSave" runat="server" text="Save"></asp:button><asp:button id="btnCancel" runat="server" causesvalidation="False" text="Cancel"></asp:button><asp:button id="btnNew" runat="server" text="Add new child"></asp:button>
				<asp:button id="btnDelete" runat="server" text="Delete" causesvalidation="False"></asp:button></div>
		</form>
	</body>
</html>
