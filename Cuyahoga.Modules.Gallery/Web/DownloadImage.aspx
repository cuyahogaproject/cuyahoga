<%@ Page language="c#" Codebehind="DownloadImage.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Gallery.Web.DownloadImage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>DownloadImage</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript">
		function show()
		{
			var obj = document.getElementById( "loading" ); 
			obj.style.visibility = "hidden";
			obj = document.getElementById( "main" ); 
			obj.style.visibility = "visible";
		}
		</script>
	</HEAD>
	<body id="body" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="GridLayout"
		runat="server">
		<script language="JavaScript">
		<!--
		//Disable right click script III- By Renigade (renigade@mediaone.net)
		//For full source code, visit http://www.dynamicdrive.com

		var message="";
		///////////////////////////////////
		function clickIE() {if (document.all) {(message);return false;}}
		function clickNS(e) {if 
		(document.layers||(document.getElementById&&!document.all)) {
		if (e.which==2||e.which==3) {(message);return false;}}}
		if (document.layers) 
		{document.captureEvents(Event.MOUSEDOWN);document.onmousedown=clickNS;}
		else{document.onmouseup=clickNS;document.oncontextmenu=clickIE;}

		document.oncontextmenu=new Function("return false")
		// --> 
		</script>
		<form id="Form1" method="post" runat="server">
			<div id="loading" style="Z-INDEX: 1; LEFT: 0px; WIDTH: 100%; POSITION: absolute; TOP: 0px; HEIGHT: 100%">
				<img width="126" height="22" border="0" src="../../Modules/xpGallery/images/overlay.png"
					align="middle">
			</div>
			<div id="main" style="Z-INDEX: 5; LEFT: 0px; VISIBILITY: hidden; WIDTH: 100%; POSITION: absolute; TOP: 0px; HEIGHT: 100%">
				<table width="100%" height="100%" border="0" style="BACKGROUND-IMAGE: url(../../Modules/xpGallery/images/overlay.png); BACKGROUND-REPEAT: repeat">
					<tr height="20">
						<td width="100%" align="right" valign="top">
							<img width="20" height="20" border="0" src="../../Modules/xpGallery/images/close.gif"
								onclick="window.close();" onmouseover="this.style.cursor='pointer';">
						</td>
					</tr>
					<tr>
						<td valign="middle" align="center" width="100%" height="600">
							<img width="800" height="600" border="0" id="imgMain" runat="server" align="middle" onclick="return false;"
								onmousedown="return false;">
						</td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
