<%@ register tagprefix="cc1" namespace="Cuyahoga.ServerControls" assembly="Cuyahoga.ServerControls" %>
<%@ control language="c#" autoeventwireup="false" codebehind="ForumViewProfile.ascx.cs"
	inherits="Cuyahoga.Modules.Forum.ForumViewProfile" targetschema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:placeholder id="phForumTop" runat="server"></asp:placeholder>
<table class="grid" id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
	<tr class="gridsubheader">
		<td width="30%">
			<asp:label id="ltProfile" runat="server">Profile</asp:label></td>
		<td width="70%">
		</td>
	</tr>
	<tr>
		<td width="30%">
			<asp:label id="lblUserName" runat="server">Label</asp:label></td>
		<td width="70%">
			<asp:literal id="ltrUserName" runat="server"></asp:literal></td>
	</tr>
	<tr>
		<td>
			<asp:label id="lblRealName" runat="server">Label</asp:label></td>
		<td>
			<asp:literal id="ltrRealName" runat="server"></asp:literal></td>
	</tr>
	<tr>
		<td style="height: 21px">
			<asp:label id="lblLocation" runat="server">Label</asp:label></td>
		<td style="height: 21px">
			<asp:literal id="ltrLocation" runat="server"></asp:literal></td>
	</tr>
	<tr>
		<td style="height: 21px">
			<asp:label id="lblOccupation" runat="server">Label</asp:label></td>
		<td style="height: 21px">
			<asp:literal id="ltroccupation" runat="server"></asp:literal></td>
	</tr>
	<tr>
		<td>
			<asp:label id="lblInterest" runat="server">Label</asp:label></td>
		<td>
			<asp:literal id="ltrInterest" runat="server"></asp:literal></td>
	</tr>
	<tr>
		<td>
			<asp:label id="lblGender" runat="server">Label</asp:label></td>
		<td>
			<asp:literal id="ltrGender" runat="server"></asp:literal></td>
	</tr>
	<tr>
		<td>
			<asp:label id="lblHomepage" runat="server">Label</asp:label></td>
		<td>
			<asp:literal id="ltrHomepage" runat="server"></asp:literal></td>
	</tr>
	<tr>
		<td>
			<asp:label id="lblMSN" runat="server">Label</asp:label></td>
		<td>
			<asp:literal id="ltrMSN" runat="server"></asp:literal></td>
	</tr>
	<tr>
		<td>
			<asp:label id="lblYahooMessenger" runat="server">Label</asp:label></td>
		<td>
			<asp:literal id="ltrYahooMessenger" runat="server"></asp:literal></td>
	</tr>
	<tr>
		<td>
			<asp:label id="lblAIMName" runat="server">Label</asp:label></td>
		<td>
			<asp:literal id="ltrAIMName" runat="server"></asp:literal></td>
	</tr>
	<tr>
		<td>
			<asp:label id="lblICQNumber" runat="server">Label</asp:label></td>
		<td>
			<asp:literal id="ltrICQNumber" runat="server"></asp:literal></td>
	</tr>
</table>
<asp:placeholder id="phForumFooter" runat="server"></asp:placeholder>
