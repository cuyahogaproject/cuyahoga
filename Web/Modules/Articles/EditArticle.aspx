<%@ Page language="c#" Codebehind="EditArticle.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Modules.Articles.EditArticle" ValidateRequest="false" %>
<%@ Register TagPrefix="cc1" Namespace="Cuyahoga.ServerControls" Assembly="Cuyahoga.ServerControls" %>
<%@ Register TagPrefix="ew" Namespace="eWorld.UI" Assembly="eWorld.UI" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>EditArticle</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<div id="moduleadminpane">
				<h1>Edit Article</h1>
				<div class="group">
					<h4>Article</h4>
					<table>
						<tr>
							<td style="WIDTH: 100px">Title</td>
							<td><asp:textbox id="txtTitle" runat="server" width="650px"></asp:textbox><asp:requiredfieldvalidator id="rfvTitle" runat="server" cssclass="validator" display="Dynamic" errormessage="The title is required" enableclientscript="False" controltovalidate="txtTitle"></asp:requiredfieldvalidator></td></tr>
						<tr>
							<td style="WIDTH: 100px">Summary</td>
							<td><asp:textbox id="txtSummary" runat="server" width="650px" height="60px" textmode="MultiLine"></asp:textbox></td></tr>
						<tr>
							<td style="WIDTH: 100px">Content</td>
							<td><cc1:cuyahogaeditor id="cedContent" runat="server" width="650px" stripallscripting="True" allowhtmlmode="False" downlevelrows="20" downlevelcols="80" imagedir="~/UserFiles/Images" supportfolder="~/Support/FreeTextBox/"></cc1:cuyahogaeditor><asp:requiredfieldvalidator id="rfvContent" runat="server" cssclass="validator" display="Dynamic" errormessage="Content is required" enableclientscript="False" controltovalidate="cedContent"></asp:requiredfieldvalidator></td></tr>
						<tr>
							<td style="WIDTH: 100px">Category</td>
							<td><asp:dropdownlist id="ddlCategory" runat="server" width="200px"></asp:dropdownlist>&nbsp;or
								enter a new category: <asp:textbox id="txtCategory" runat="server" width="200px"></asp:textbox>
							</td>
						</tr>
					</table>
				</div>
				<div class="group">
					<h4>Publishing</h4>
					<table>
						<tr>
							<td style="WIDTH: 100px">Syndicate</td>
							<td><asp:checkbox id="chkSyndicate" runat="server" checked="True"></asp:checkbox></td></tr>
						<tr>
							<td style="WIDTH: 100px">Date online</td>
							<td><ew:calendarpopup id="calDateOnline" runat="server" width="100px" externalresourcepath="~/Support/eWorld/eWorld_UI_CalendarPopup.js" useexternalresource="True">
									<weekdaystyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="White">
									</weekdaystyle>

									<monthheaderstyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="Yellow">
									</monthheaderstyle>

									<offmonthstyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Gray" backcolor="AntiqueWhite">
									</offmonthstyle>

									<gototodaystyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="White">
									</gototodaystyle>

									<todaydaystyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="LightGoldenrodYellow">
									</todaydaystyle>

									<dayheaderstyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="Orange">
									</dayheaderstyle>

									<weekendstyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="LightGray">
									</weekendstyle>

									<selecteddatestyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="Yellow">
									</selecteddatestyle>

									<cleardatestyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="White">
									</cleardatestyle>

									<holidaystyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="White">
									</holidaystyle>
								</ew:calendarpopup></td></tr>
						<tr>
							<td style="WIDTH: 100px">Date offline</td>
							<td><ew:calendarpopup id="calDateOffline" runat="server" width="100px" useexternalresource="True" externalresourcepath="~/Support/eWorld/eWorld_UI_CalendarPopup.js">
									<weekdaystyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="White">
									</weekdaystyle>

									<monthheaderstyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="Yellow">
									</monthheaderstyle>

									<offmonthstyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Gray" backcolor="AntiqueWhite">
									</offmonthstyle>

									<gototodaystyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="White">
									</gototodaystyle>

									<todaydaystyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="LightGoldenrodYellow">
									</todaydaystyle>

									<dayheaderstyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="Orange">
									</dayheaderstyle>

									<weekendstyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="LightGray">
									</weekendstyle>

									<selecteddatestyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="Yellow">
									</selecteddatestyle>

									<cleardatestyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="White">
									</cleardatestyle>

									<holidaystyle font-size="XX-Small" font-names="Verdana,Helvetica,Tahoma,Arial" forecolor="Black" backcolor="White">
									</holidaystyle>
								</ew:calendarpopup></td></tr></table>
				</div>
				<p><asp:button id="btnSave" runat="server" text="Save"></asp:button><asp:button id="btnDelete" runat="server" text="Delete" visible="False"></asp:button><input id="btnCancel" type="button" value="Cancel" runat="server"></p></div></form>
	</body>
</html>
