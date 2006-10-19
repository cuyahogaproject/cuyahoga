<%@ Page language="c#" Codebehind="UserEdit.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Admin.UserEdit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>UserEdit</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<script type="text/javascript"> <!--
				function confirmDeleteUser(userId)
				{
					if (confirm("Are you sure you want to delete this user?"))
						document.location.href = "UserEdit.aspx?UserId=" + userId + "&Action=Delete";
				}
				// -->
			</script>
			<div class="group">
				<h4>General</h4>
				<table>
					<tr>
						<td style="WIDTH: 200px">Username</td>
						<td><asp:textbox id="txtUsername" runat="server" width="200px"></asp:textbox><asp:label id="lblUsername" runat="server" visible="False"></asp:label><asp:requiredfieldvalidator id="rfvUsername" runat="server" errormessage="Username is required" cssclass="validator"
								display="Dynamic" enableclientscript="False" controltovalidate="txtUsername"></asp:requiredfieldvalidator></td>
					</tr>
					<tr>
						<td>Firstname</td>
						<td><asp:textbox id="txtFirstname" runat="server" width="200px"></asp:textbox></td>
					</tr>
					<tr>
						<td>Lastname</td>
						<td><asp:textbox id="txtLastname" runat="server" width="200px"></asp:textbox></td>
					</tr>
					<tr>
						<td>Email</td>
						<td><asp:textbox id="txtEmail" runat="server" width="200px"></asp:textbox><asp:requiredfieldvalidator id="rfvEmail" runat="server" controltovalidate="txtEmail" enableclientscript="False"
								display="Dynamic" cssclass="validator" errormessage="Email is required"></asp:requiredfieldvalidator><asp:regularexpressionvalidator id="revEmail" runat="server" controltovalidate="txtEmail" enableclientscript="False"
								display="Dynamic" cssclass="validator" errormessage="Invalid email" validationexpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:regularexpressionvalidator></td>
					</tr>
					<tr>
						<td>Website</td>
						<td>
							<asp:textbox id="txtWebsite" runat="server" width="200px"></asp:textbox></td>
					</tr>
					<tr>
						<td>Active</td>
						<td><asp:checkbox id="chkActive" runat="server"></asp:checkbox></td>
					</tr>
					<tr>
						<td>Timezone</td>
						<td>
							<asp:dropdownlist id="ddlTimeZone" runat="server"></asp:dropdownlist></td>
					</tr>
					<tr>
						<td>Password</td>
						<td><asp:textbox id="txtPassword1" runat="server" width="200px" textmode="Password"></asp:textbox></td>
					</tr>
					<tr>
						<td>Confirm password</td>
						<td><asp:textbox id="txtPassword2" runat="server" width="200px" textmode="Password"></asp:textbox><asp:comparevalidator id="covPassword" runat="server" controltovalidate="txtPassword1" enableclientscript="False"
								display="Dynamic" cssclass="validator" errormessage="Both passwords must be the same" controltocompare="txtPassword2"></asp:comparevalidator></td>
					</tr>
				</table>
			</div>
			<div class="group">
				<h4>Roles</h4>
				<table class="tbl">
					<asp:repeater id="rptRoles" runat="server">
						<headertemplate>
							<tr>
								<th>
									Role</th>
								<th>
								</th>
							</tr>
						</headertemplate>
						<itemtemplate>
							<tr>
								<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
								<td style="text-align:center">
									<asp:checkbox id="chkRole" runat="server"></asp:checkbox></td>
							</tr>
						</itemtemplate>
					</asp:repeater></table>
			</div>
			<div><asp:button id="btnSave" runat="server" text="Save"></asp:button><asp:button id="btnCancel" runat="server" text="Cancel" causesvalidation="False"></asp:button><asp:button id="btnDelete" runat="server" text="Delete"></asp:button></div>
		</form>
	</body>
</html>
