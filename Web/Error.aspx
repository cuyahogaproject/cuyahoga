<%@ Page language="c#" Codebehind="Error.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Web.Error" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Error</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style>
			body
			{
				margin: 0px;
				background-color: #EEEEEE;
			}

			p, div
			{
				font-family: Arial, Helvetica;
				font-size: 8pt;
				color: #333333;
			}

			h1
			{
				font-family: Arial, Helvetica;
				font-size: 12pt;
				font-weight: bold;
				background-color: #EEEEEE;
				width: 100%;
				border-bottom: solid 1px #CCCCCC;
				padding: 1px;
			}
			#errorbox
			{
				padding: 10px;
				margin:30px; 
				background-color:#fff;
			}
		</style>
	</head>
	<body>

		<form id="Form1" method="post" runat="server">
			<div id="errorbox">
				<h1><asp:label id="lblTitle" runat="server"></asp:label></h1>
				<p><asp:label id="lblError" runat="server"></asp:label></p>
			</div>
		</form>

	</body>
</html>
