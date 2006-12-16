<%@ Page language="c#" Codebehind="AdminGalleries.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Gallery.Web.AdminGalleries" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AdminGalleries</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Gallery management</h1>
				<p>
					<table class="tbl">
						<asp:repeater id="rptGalleries" runat="server">
							<headertemplate>
								<tr>
									<th>
										Include
									</th>
									<th>
										Position</th>
									<th>
										Name</th>
									<th>
										Title</th>
									<th>
										Updated</th>
									<th>
										Owner</th>
									<th>
										Images</th>
									<th>
									</th>
								</tr>
							</headertemplate>
							<itemtemplate>
								<tr>
									<td><%# DataBinder.Eval(Container.DataItem, "Include") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "Sequence") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
									<td><%# DataBinder.Eval(Container.DataItem, "Title") %></td>
									<td>
										<asp:literal id="litDateUpdated" runat="server"></asp:literal></td>
									<td><%# DataBinder.Eval(Container.DataItem, "Owner.UserName") %></td>
									<td>
										<asp:hyperlink id="hplEdit" runat="server">Manage</asp:hyperlink>
										<asp:hyperlink id="hpPhotos" runat="server">Photos</asp:hyperlink>
										<asp:hyperlink id="hplComments" runat="server">Comments</asp:hyperlink>
									</td>
								</tr>
							</itemtemplate>
						</asp:repeater>
					</table>
				</p>
				<input id="btnNew" type="button" value="New Gallery" runat="server" NAME="btnNew">
			</div>
		</form>
	</body>
</HTML>
