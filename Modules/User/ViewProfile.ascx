<%@ Control Language="c#" AutoEventWireup="True" Codebehind="ViewProfile.ascx.cs" Inherits="Cuyahoga.Modules.User.ViewProfile" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<h3><asp:literal id="litTitle" runat="server"></asp:literal></h3>

<asp:label id="lblError" runat="server" enableviewstate="False" cssclass="error" visible="False"></asp:label><br/>
<table class="tbl">
	<tr>
		<td style="WIDTH: 100px"><%= GetText("USERNAME") %></td>
		<td class="tblvalue"><asp:label id="lblUsername" runat="server"></asp:label></td>
		<td style="WIDTH: 100px"><%= GetText("REGISTEREDON") %></td>
		<td class="tblvalue"><asp:label id="lblRegisteredOn" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td><%= GetText("FIRSTNAME") %></td>
		<td class="tblvalue"><asp:label id="lblFirstname" runat="server"></asp:label></td>
		<td><%= GetText("LASTLOGINON") %></td>
		<td class="tblvalue"><asp:label id="lblLastLogin" runat="server"></asp:label></td>
	</tr>
	<tr>
		<td><%= GetText("LASTNAME") %></td>
		<td class="tblvalue"><asp:label id="lblLastname" runat="server"></asp:label></td>
		<td>&nbsp;</td>
		<td class="tblvalue">&nbsp;</td>
	</tr>
	<tr>
		<td><%= GetText("WEBSITE") %></td>
		<td class="tblvalue"><asp:hyperlink id="hplWebsite" runat="server" target="_blank"></asp:hyperlink></td>
		<td>&nbsp;</td>
		<td class="tblvalue">&nbsp;</td>
	</tr>
</table>
