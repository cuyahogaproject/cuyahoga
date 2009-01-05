<%@ Page language="c#" Codebehind="SiteAliasEdit.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.SiteAliasEdit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
  <head>
    <title>SiteAliasEdit</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
  <body MS_POSITIONING="FlowLayout">
	
    <form id="Form1" method="post" runat="server">
			<div class="group">
				<h4>General</h4>
				<table>
					<tr>
						<td style="WIDTH: 100px">Url (incl. http://)</td>
						<td><asp:textbox id="txtUrl" runat="server" width="300px" maxlength="100"></asp:textbox><asp:requiredfieldvalidator id="rfvName" runat="server" enableclientscript="False" controltovalidate="txtUrl"
								cssclass="validator" display="Dynamic">Url is required</asp:requiredfieldvalidator></td>
					</tr>
					<tr>
						<td>Entry node</td>
						<td>
<asp:DropDownList id=ddlEntryNodes runat="server"></asp:DropDownList>
						
						</td>
					</tr>
				</table>
			</div>
			<div>
				<asp:button id="btnSave" runat="server" text="Save"></asp:button>
				<asp:button id="btnCancel" runat="server" text="Cancel" causesvalidation="False"></asp:button>
				<asp:button id="btnDelete" runat="server" text="Delete" causesvalidation="False"></asp:button>
			</div>
     </form>
	
  </body>
</html>
