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
			<script type="text/javascript"> <!--
				function confirmDeleteNode()
				{
					return confirm("Are you sure you want to delete this node?");
				}
				// -->
			</script>
			<p>Manage the properties of the node (page). Use the buttons on the bottom
			of the page to save or delete the page or to add a new child node underneath this node.</p>
			<div class="group">
				<h4>General</h4>
				<table>
					<tr>
						<td style="WIDTH: 100px">Node title</td>
						<td><asp:textbox id="txtTitle" runat="server" width="300px"></asp:textbox><asp:requiredfieldvalidator id="rfvTitle" runat="server" errormessage="Title is required" display="Dynamic"
								cssclass="validator" controltovalidate="txtTitle" enableclientscript="False"></asp:requiredfieldvalidator></td>
					</tr>
					<tr>
						<td>Friendly url</td>
						<td><asp:textbox id="txtShortDescription" runat="server" width="300px" tooltip="You can use this for 'nice' links ([shortdescription].aspx). Make sure it's unique!"></asp:textbox>.aspx&nbsp;<asp:regularexpressionvalidator id="revShortDescription" runat="server" errormessage="No spaces are allowed" display="Dynamic"
								cssclass="validator" controltovalidate="txtShortDescription" enableclientscript="False" validationexpression="\S+"></asp:regularexpressionvalidator><asp:requiredfieldvalidator id="rfvShortDescription" runat="server" errormessage="Short description is required"
								display="Dynamic" controltovalidate="txtShortDescription" enableclientscript="False"></asp:requiredfieldvalidator></td>
					</tr>
					<tr>
						<td>Parent node</td>
						<td><asp:label id="lblParentNode" runat="server"></asp:label></td>
					</tr>
					<tr>
						<td>Position</td>
						<td><asp:imagebutton id="btnUp" runat="server" imageurl="~/Admin/Images/upred.gif" causesvalidation="False"
								alternatetext="Move up"></asp:imagebutton><asp:imagebutton id="btnDown" runat="server" imageurl="~/Admin/Images/downred.gif" causesvalidation="False"
								alternatetext="Move down"></asp:imagebutton><asp:imagebutton id="btnLeft" runat="server" imageurl="~/Admin/Images/leftred.gif" causesvalidation="False"
								alternatetext="Move left"></asp:imagebutton><asp:imagebutton id="btnRight" runat="server" imageurl="~/Admin/Images/rightred.gif" causesvalidation="False"
								alternatetext="Move right"></asp:imagebutton></td>
					</tr>
					<tr>
						<td>Culture</td>
						<td><asp:dropdownlist id="ddlCultures" runat="server"></asp:dropdownlist></td>
					</tr>
					<tr>
						<td>Show in navigation</td>
						<td><asp:checkbox id="chkShowInNavigation" runat="server"></asp:checkbox></td>
					</tr>
					<tr>
						<td>Meta description</td>
						<td>
							<asp:textbox id="txtMetaDescription" runat="server" maxlength="500" textmode="MultiLine"
								width="400px" height="35px"></asp:textbox></td>
					</tr>
					<tr>
						<td>Meta keywords</td>
						<td>
							<asp:textbox id="txtMetaKeywords" runat="server" maxlength="500" textmode="MultiLine"
								width="400px" height="35px"></asp:textbox></td>
					</tr>
					<tr>
						<td></td>
						<td><asp:checkbox id="chkLink" runat="server" autopostback="True" text="The node is a link to an external url"></asp:checkbox><asp:panel id="pnlLink" runat="server" visible="False">
								<table>
									<tr>
										<td style="width:60px">Url</td>
										<td>
											<asp:textbox id="txtLinkUrl" runat="server" width="400px"></asp:textbox></td>
									</tr>
									<tr>
										<td>Target</td>
										<td>
											<asp:dropdownlist id="ddlLinkTarget" runat="server">
												<asp:listitem value="Self">Same window</asp:listitem>
												<asp:listitem value="New">New window</asp:listitem>
											</asp:dropdownlist></td>
									</tr>
								</table>
							</asp:panel></td>
					</tr>
				</table>
			</div>
			<asp:panel id="pnlTemplate" runat="server" cssclass="group">
				<h4>Template</h4>
				<table>
					<tr>
						<td style="WIDTH: 100px">&nbsp;</td>
						<td>
							<asp:dropdownlist id="ddlTemplates" runat="server" autopostback="True"></asp:dropdownlist></td>
					</tr>
				</table>
			</asp:panel><asp:panel id="pnlMenus" runat="server" cssclass="group" visible="False">
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
			</asp:panel><asp:panel id="pnlSections" runat="server" cssclass="group">
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
									<asp:hyperlink id="hplSectionUp" imageurl="~/Admin/Images/upred.gif" visible="False" enableviewstate="False"
										runat="server">Move up</asp:hyperlink>
									<asp:hyperlink id="hplSectionDown" imageurl="~/Admin/Images/downred.gif" visible="False" enableviewstate="False"
										runat="server">Move down</asp:hyperlink>
								</td>
								<td>
									<asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink>
									<asp:linkbutton id="lbtDetach" runat="server" causesvalidation="False" commandname="Detach" commandargument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>Detach</asp:linkbutton>
									<asp:linkbutton id="lbtDelete" runat="server" causesvalidation="False" commandname="Delete" commandargument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>Delete</asp:linkbutton>
								</td>
							</tr>
						</itemtemplate>
					</asp:repeater></table>
				<asp:hyperlink id="hplNewSection" runat="server" visible="False">Add section</asp:hyperlink>
			</asp:panel>
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
					</asp:repeater></table>
				<br/>
				<asp:checkbox id="chkPropagateToSections" runat="server" text="Propagate security settings to sections"></asp:checkbox><br/>
				<asp:checkbox id="chkPropagateToChildNodes" runat="server" text="Propagate security settings to child nodes"></asp:checkbox></div>
			<div><asp:button id="btnSave" runat="server" text="Save"></asp:button><asp:button id="btnCancel" runat="server" causesvalidation="False" text="Cancel"></asp:button><asp:button id="btnNew" runat="server" text="Add new child"></asp:button><asp:button id="btnDelete" runat="server" causesvalidation="False" text="Delete"></asp:button></div>
		</form>
	</body>
</html>
