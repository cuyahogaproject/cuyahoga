<%@ Page language="c#" Codebehind="NodeEdit.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.NodeEdit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 TRANSITIONAL//EN" >
<html>
  <head>
		<title>Cuyahoga Site Administration</title>
<meta content="Microsoft Visual Studio .NET 7.1" name=GENERATOR>
<meta content=C# name=CODE_LANGUAGE>
<meta content=JavaScript name=vs_defaultClientScript>
<meta content=http://schemas.microsoft.com/intellisense/ie5 name=vs_targetSchema><LINK href="../Css/Admin.css" type=text/css rel=stylesheet >
  </head>
<body>
<form id=Form1 method=post runat="server">
<script language=javascript> <!--
				function confirmDeleteNode()
				{
					return confirm("Are you sure you want to delete this node?");
				}

				function confirmDeleteSection(sectionId, nodeId)
				{
					if (confirm("Are you sure you want to delete this section?"))
						document.location.href = "NodeEdit.aspx?NodeId=" + nodeId + "&SectionId=" + sectionId + "&Action=Delete";
				}
				// -->
			</script>

<div class=group>
<h4>General</H4>
<table>
  <tr>
    <td style="WIDTH: 100px">Node title</TD>
    <td><asp:textbox id=txtTitle runat="server" width="300px"></asp:textbox><asp:requiredfieldvalidator id=rfvTitle runat="server" enableclientscript="False" controltovalidate="txtTitle" cssclass="validator" display="Dynamic" errormessage="Title is required"></asp:requiredfieldvalidator></TD></TR>
  <tr>
    <td>Navigation path prefix</TD>
    <td><asp:textbox id=txtShortDescriptionPrefix runat="server" Width="150px"></asp:TextBox><asp:regularexpressionvalidator id=revShortDescriptionPrefix runat="server" enableclientscript="False" controltovalidate="txtShortDescriptionPrefix" cssclass="validator" display="Dynamic" errormessage="No spaces are allowed" validationexpression="\S+"></asp:regularexpressionvalidator></TD></TR>
  <tr>
    <td>Short description</TD>
    <td><asp:textbox id=txtShortDescription runat="server" width="150px" tooltip="You can use this for 'nice' links ([shortdescription].aspx). Make sure it's unique."></asp:textbox><asp:regularexpressionvalidator id=revShortDescription runat="server" enableclientscript="False" controltovalidate="txtShortDescription" cssclass="validator" display="Dynamic" errormessage="No spaces are allowed" validationexpression="\S+"></asp:regularexpressionvalidator><asp:requiredfieldvalidator id=rfvShortDescription runat="server" ErrorMessage="Short description is required" ControlToValidate="txtShortDescription" Display="Dynamic" EnableClientScript="False"></asp:RequiredFieldValidator></TD></TR>
  <tr>
    <td>Parent node</TD>
    <td><asp:label id=lblParentNode runat="server"></asp:label></TD></TR>
  <tr>
    <td>Position</TD>
    <td><asp:imagebutton id=btnUp runat="server" alternatetext="Move up" causesvalidation="False" imageurl="../Images/upred.gif"></asp:imagebutton><asp:imagebutton id=btnDown runat="server" alternatetext="Move down" causesvalidation="False" imageurl="../Images/downred.gif"></asp:imagebutton><asp:imagebutton id=btnLeft runat="server" alternatetext="Move left" causesvalidation="False" imageurl="../Images/leftred.gif"></asp:imagebutton><asp:imagebutton id=btnRight runat="server" alternatetext="Move right" causesvalidation="False" imageurl="../Images/rightred.gif"></asp:imagebutton></TD></TR></TABLE></DIV>
<div class=group>
<h4>Template</H4>
<table>
  <tr>
    <td style="WIDTH: 100px">&nbsp;</TD>
    <td><asp:dropdownlist id=ddlTemplates runat="server" autopostback="True" visible="false">
							</asp:dropdownlist><asp:label id=lblTemplates runat="server" visible="false"></asp:label></TD></TR></TABLE></DIV>
<div class=group>
<h4>Sections</H4>
<table class=tbl><asp:repeater id=rptSections runat="server">
						<headertemplate>
							<tr>
								<th>Section title</th>
								<th>Module type</th>
								<th>Placeholder</th>
								<th>Cache duration</th>
								<th>Position</th>
								<th>Actions</th>
							</tr>
						</headertemplate>
						<itemtemplate>
							<tr>
								<td><%# DataBinder.Eval(Container.DataItem, "Title") %></td>
								<td><%# DataBinder.Eval(Container.DataItem, "Module.Name") %></td>
								<td><%# DataBinder.Eval(Container.DataItem, "PlaceholderId") %></td>
								<td style="text-align:right"><%# DataBinder.Eval(Container.DataItem, "CacheDuration") %></td>
								<td>
									<asp:hyperlink id="hplSectionUp" imageurl="../Images/upred.gif" visible="False" enableviewstate="False" runat="server">Move up</asp:hyperlink>
									<asp:hyperlink id="hplSectionDown" imageurl="../Images/downred.gif" visible="False" enableviewstate="False" runat="server">Move down</asp:hyperlink>
								</td>
								<td>
									<asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink>
									<asp:hyperlink id="hplDetach" runat="server">Detach</asp:hyperlink>
									<asp:hyperlink id="hplDelete" runat="server">Delete</asp:hyperlink>
								</td>
							</tr>
						</itemtemplate>
					</asp:repeater></TABLE><asp:hyperlink id=hplNewSection runat="server" visible="False">Add section</asp:hyperlink></DIV>
<div class=group>
<h4>Authorization</H4>
<table class=tbl>
					<asp:repeater id="rptRoles" runat="server">
						<headertemplate>
							<tr>
								<th>Role</th>
								<th>View allowed</th>
								<th>Edit allowed</th>
							</tr>
						</headertemplate>
						<itemtemplate>
							<tr>
								<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
								<td style="text-align:center"><asp:checkbox id="chkViewAllowed" runat="server"></asp:checkbox></td>
								<td style="text-align:center"><asp:checkbox id="chkEditAllowed" runat="server"></asp:checkbox></td>
							</tr>
						</itemtemplate>
					</asp:repeater></TABLE></DIV>
			<div><asp:button id="btnSave" runat="server" text="Save"></asp:button><asp:button id="btnCancel" runat="server" causesvalidation="False" text="Cancel"></asp:button><asp:button id="btnNew" runat="server" text="Add new child"></asp:button>
				<asp:button id="btnDelete" runat="server" text="Delete" causesvalidation="False"></asp:button></div></FORM>
	</body>
</html>
