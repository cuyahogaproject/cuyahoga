<%@ Page language="c#" Codebehind="Sections.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.Sections" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 TRANSITIONAL//EN" >
<html>
	<head>
		<title>Sections</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<p>Manage sections that are not related to a node. This can be detached sections or 
				sections that are connected to&nbsp;one or more&nbsp;templates.</p>
			<table class="tbl">
				<asp:repeater id="rptSections" runat="server">
					<headertemplate>
						<tr>
							<th>
								Section name</th>
							<th>
								Module type</th>
							<th>
								Attached to template(s)</th>
							<th>
								Actions</th>
						</tr>
					</headertemplate>
					<itemtemplate>
						<tr>
							<td><%# DataBinder.Eval(Container.DataItem, "Title") %></td>
							<td><%# DataBinder.Eval(Container.DataItem, "ModuleType.Name") %></td>
							<td>
								<asp:literal id="litTemplates" runat="server" />
							</td>
							<td style="white-space:nowrap">
								<asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink>
								<asp:linkbutton id="lbtDelete" runat="server" causesvalidation="False" commandname="Delete" commandargument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'>Delete</asp:linkbutton>
								<asp:hyperlink id="hplAttachTemplate" runat="server">Attach to template</asp:hyperlink>
								<asp:hyperlink id="hplAttachNode" runat="server">Attach to node</asp:hyperlink>
							</td>
						</tr>
					</itemtemplate>
				</asp:repeater></table>
			<br/>
			<div><asp:button id="btnNew" runat="server" text="Add new section"></asp:button></div>
		</form>
	</body>
</html>
