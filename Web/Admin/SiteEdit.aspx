<%@ Page language="c#" Codebehind="SiteEdit.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.SiteEdit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>SiteEdit</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div class="group">
				<h4>General</h4>
				<table>
					<tr>
						<td style="WIDTH: 100px">Name</td>
						<td><asp:textbox id="txtName" runat="server" width="300px" maxlength="100"></asp:textbox><asp:requiredfieldvalidator id="rfvName" runat="server" enableclientscript="False" controltovalidate="txtName"
								cssclass="validator" display="Dynamic">Name is required</asp:requiredfieldvalidator></td>
					</tr>
					<tr>
						<td>Site url (incl. http://)</td>
						<td><asp:textbox id="txtSiteUrl" runat="server" width="300px" maxlength="100"></asp:textbox><asp:requiredfieldvalidator id="rfvSiteUrl" runat="server" enableclientscript="False" controltovalidate="txtSiteUrl"
								cssclass="validator" display="Dynamic">Site url is required</asp:requiredfieldvalidator></td>
					</tr>
					<tr>
						<td>Webmaster email</td>
						<td><asp:textbox id="txtWebmasterEmail" runat="server" maxlength="100" width="300px"></asp:textbox><asp:requiredfieldvalidator id="rfvWebmasterEmail" runat="server" errormessage="Webmaster email is required"
								cssclass="validator" display="Dynamic" controltovalidate="txtWebmasterEmail" enableclientscript="False"></asp:requiredfieldvalidator></td>
					</tr>
					<tr>
						<td>Use friendly url's</td>
						<td><asp:checkbox id="chkUseFriendlyUrls" runat="server"></asp:checkbox></td>
					</tr>
				</table>
			</div>
			<div class="group">
				<h4>Defaults</h4>
				<table>
					<tr>
						<td style="WIDTH: 100px">Template</td>
						<td><asp:dropdownlist id="ddlTemplates" runat="server" autopostback="True"></asp:dropdownlist></td>
					</tr>
					<tr>
						<td>Placeholder</td>
						<td><asp:dropdownlist id="ddlPlaceholders" runat="server"></asp:dropdownlist><em>(this 
								is the placeholder where the content of&nbsp;general pages&nbsp;is inserted)</em></td>
					</tr>
					<tr>
						<td>Culture</td>
						<td><asp:dropdownlist id="ddlCultures" runat="server"></asp:dropdownlist></td>
					</tr>
					<tr>
						<td>Role for registered users</td>
						<td><asp:dropdownlist id="ddlRoles" runat="server"></asp:dropdownlist></td>
					</tr>
					<tr>
						<td>
							Meta description</td>
						<td>
							<asp:TextBox ID="txtMetaDescription" runat="server" MaxLength="500" TextMode="MultiLine"
								Width="400px" height="70px"></asp:TextBox></td>
					</tr>
					<tr>
						<td>
							Meta keywords</td>
						<td>
							<asp:TextBox ID="txtMetaKeywords" runat="server" MaxLength="500" TextMode="MultiLine"
								Width="400px" height="70px"></asp:TextBox></td>
					</tr>
				</table>
			</div>
			<asp:panel id="pnlAliases" runat="server" cssclass="group">
				<h4>Aliases</h4>
				<table class="tbl">
					<asp:repeater id="rptAliases" runat="server">
						<headertemplate>
							<tr>
								<th>
									Alias url</th>
								<th>
									Entry Node</th>
								<th>
									&nbsp;</th>
							</tr>
						</headertemplate>
						<itemtemplate>
							<tr>
								<td><%# DataBinder.Eval(Container.DataItem, "Url") %></td>
								<td>
									<asp:label id="lblEntryNode" runat="server"></asp:label></td>
								<td>
									<asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink></td>
							</tr>
						</itemtemplate>
					</asp:repeater></table>
				<asp:hyperlink id="hplNewAlias" runat="server">Add alias</asp:hyperlink>
			</asp:panel>
			<div>
				<asp:button id="btnSave" runat="server" text="Save"></asp:button>
				<asp:button id="btnCancel" runat="server" text="Cancel" causesvalidation="False"></asp:button>
				<asp:button id="btnDelete" runat="server" text="Delete" causesvalidation="False"></asp:button>
			</div>
		</form>
	</body>
</html>
