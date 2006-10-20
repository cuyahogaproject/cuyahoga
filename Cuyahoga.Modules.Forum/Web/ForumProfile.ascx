<%@ control language="c#" autoeventwireup="false" codebehind="ForumProfile.ascx.cs"
	inherits="Cuyahoga.Modules.Forum.ForumProfile" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ register tagprefix="cc1" namespace="Cuyahoga.ServerControls" assembly="Cuyahoga.ServerControls" %>
<asp:placeholder id="phForumTop" runat="server"></asp:placeholder>
<asp:panel id="pnlProfile" runat="server" cssclass="articlesub">
	<table id="tblProfile" cellspacing="1" cellpadding="1" width="100%" border="0">
		<tr>
			<td style="width: 298px" width="298">
				<asp:label id="lblUserName" runat="server">Label</asp:label></td>
			<td>
				<asp:literal id="ltlUserName" runat="server"></asp:literal></td>
		</tr>
		<tr>
			<td style="width: 298px">
				<asp:label id="lblRealName" runat="server">Label</asp:label></td>
			<td>
				<asp:literal id="ltlRealName" runat="server"></asp:literal></td>
		</tr>
		<tr>
			<td style="width: 298px">
				<asp:label id="lblLocation" runat="server">Label</asp:label></td>
			<td>
				<asp:textbox id="txtLocation" runat="server" columns="40"></asp:textbox></td>
		</tr>
		<tr>
			<td style="width: 298px">
				<asp:label id="lblOccupation" runat="server">Label</asp:label></td>
			<td>
				<asp:textbox id="txtOccupation" runat="server" columns="40"></asp:textbox></td>
		</tr>
		<tr>
			<td style="width: 298px">
				<asp:label id="lblInterest" runat="server">Label</asp:label></td>
			<td>
				<asp:textbox id="txtInterest" runat="server" columns="40"></asp:textbox></td>
		</tr>
		<tr>
			<td style="width: 298px">
				<asp:label id="lblGender" runat="server">Label</asp:label></td>
			<td>
				<asp:radiobutton id="rbMale" runat="server" text="Male" groupname="grpGender"></asp:radiobutton>&nbsp;
				<asp:radiobutton id="rbFemale" runat="server" text="Female" groupname="grpGender"></asp:radiobutton></td>
		</tr>
		<tr>
			<td style="width: 298px">
				<asp:label id="lblHomePage" runat="server">Label</asp:label></td>
			<td>
				<asp:textbox id="txtHomepage" runat="server" columns="40"></asp:textbox></td>
		</tr>
		<tr>
			<td style="width: 298px">
				<asp:label id="lblMSN" runat="server">Label</asp:label></td>
			<td>
				<asp:textbox id="txtMSN" runat="server" columns="40"></asp:textbox></td>
		</tr>
		<tr>
			<td style="width: 298px">
				<asp:label id="lblYahooMessenger" runat="server">Label</asp:label></td>
			<td>
				<asp:textbox id="txtYahooMessenger" runat="server" columns="40"></asp:textbox></td>
		</tr>
		<tr>
			<td style="width: 298px">
				<asp:label id="lblAIMName" runat="server">Label</asp:label></td>
			<td>
				<asp:textbox id="txtAIMName" runat="server" columns="40"></asp:textbox></td>
		</tr>
		<tr>
			<td style="width: 298px">
				<asp:label id="lblICQNumber" runat="server">Label</asp:label></td>
			<td>
				<asp:textbox id="txtICQNumber" runat="server" columns="40"></asp:textbox></td>
		</tr>
		<tr>
			<td style="width: 298px">
				<asp:label id="lblTimeZone" runat="server">Label</asp:label></td>
			<td>
				<asp:dropdownlist id="ddlTimeZone" runat="server" datavaluefield="Value" datatextfield="Name">
				</asp:dropdownlist></td>
		</tr>
		<tr>
			<td style="width: 298px">
				<asp:label id="lblAvartar" runat="server" visible="False">Label</asp:label></td>
			<td>
				<asp:hyperlink id="HyperLink1" runat="server" visible="False">HyperLink</asp:hyperlink></td>
		</tr>
		<tr>
			<td valign="top">
				<asp:label id="lblSignature" runat="server">Label</asp:label></td>
			<td>
				<asp:textbox id="txtSignature" runat="server" columns="40" rows="3" textmode="MultiLine"></asp:textbox></td>
		</tr>
		<tr>
			<td valign="top">
			</td>
			<td>
				<asp:button id="btnSave" runat="server" cssclass="forum" text="Save"></asp:button>&nbsp;
				<asp:button id="btnCancel" runat="server" cssclass="forum" text="Cancel" causesvalidation="False">
				</asp:button></td>
		</tr>
	</table>
</asp:panel>
<asp:placeholder id="phForumFooter" runat="server"></asp:placeholder>
