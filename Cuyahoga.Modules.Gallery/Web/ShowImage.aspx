<%@ Page language="c#" Codebehind="ShowImage.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Gallery.Web.ShowImage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ShowImage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" type="text/css" href="../../Templates/xp-rience/css/xperience.css">
		<link rel="stylesheet" type="text/css" href='<%= ResolveUrl("~/Modules/xpGallery/js/yui/css/slider.css")%>'>
		<asp:Literal Id="litJavaScripts" runat="Server"></asp:Literal>
		<!-- Namespace source file -->
		<script language="javascript">
			var slider;
			function sliderInit() {
				slider = YAHOO.widget.Slider.getHorizSlider("sliderbg", "sliderthumb", 100, 100, 25);
				// slider.animate = false;.
				slider.onChange = displayNewValue;
			}
			
			function displayNewValue(offsetFromStart) {
				var obj = document.getElementById( "hdrating" );
				obj.value = offsetFromStart;
				obj = document.getElementById( "lbrating" );
				obj.innerHTML = offsetFromStart;
				if ( offsetFromStart < 0 ) 
				{
					obj.style.color = "red";
				}
				else
				{
					obj.style.color = "green";
				}
			}
			
			function Init()
			{
				var obj = document.getElementById( "loading" );
				if ( obj != null )
				{
					obj.style.display= 'none';
					obj.style.visibility= 'hidden';
				}
				sliderInit();
			}
		</script>
	</HEAD>
	<body style="BACKGROUND-REPEAT: no-repeat; BACKGROUND-COLOR: white" onload="Init();" background="images/gallery_popup_wp.jpg">
		<form id="Form1" method="post" runat="server">
			<table class="xpLargePhoto">
				<tr>
					<td valign="middle" align="center" class="xpLargePhoto">
						<asp:Image id="image1" runat="server" BorderStyle="None"></asp:Image>
					</td>
					<td valign="middle">
						<h3>
							<asp:Literal id="litTitle" runat="server"></asp:Literal></h3>
						<p class="plaintext_black">
							<asp:Literal id="litDescription" runat="server"></asp:Literal></p>
						<table border="0" cellpadding="2" cellspacing="2">
							<tr>
								<td class="xpLabel"><%= base.GetText("SERIE") %>
								</td>
								<td><asp:Literal ID="litSerie" Runat="server"></asp:Literal></td>
							</tr>
							<tr>
								<td class="xpLabel"><%= base.GetText("CATEGORY") %>
								</td>
								<td><asp:Literal ID="litCategory" Runat="server"></asp:Literal></td>
							</tr>
							<tr>
								<td class="xpLabel"><%= base.GetText("ORIGINALSIZE") %>
								</td>
								<td><asp:Literal ID="litOriginalSize" Runat="server"></asp:Literal></td>
							</tr>
						</table>
						<h3><%= base.GetText("RATING") %></h3>
						<p class="plaintext_black">
							<%= base.GetText("MUNBEROFVIEWS") %>
							: &nbsp;
							<asp:Literal id="litViews" runat="server"></asp:Literal>
							<br>
							<%= base.GetText("CURRENTRATING") %>
							: &nbsp;
							<asp:Literal id="litRanking" runat="server"></asp:Literal>&nbsp;<%= base.GetText("NUMBEROFVOTES") %>
						</p>
						<div style="POSITION: relative">
							<div style="LEFT: 0px; WIDTH: 83px; POSITION: absolute; TOP: 0px; HEIGHT: 18px">
								<img id="imgBack" height="18" border="0" runat="server">
							</div>
							<div style="Z-INDEX: 1; LEFT: 0px; WIDTH: 83px; POSITION: absolute; TOP: 0px; HEIGHT: 18px">
								<img src='<%= ResolveUrl("~/Modules/xpGallery/Images/rating.gif")%>' height="18" width="83" border="0">
							</div>
						</div>
						<H3><%= base.GetText("VOTE") %></H3>
						<P>
							<asp:Literal ID="liVoteInfo" Runat="server"></asp:Literal>
						</P>
						<div id="sliderbg" name="sliderbg">
							<div id="sliderthumb">
								<img src='<%= base.ResolveUrl("~/Modules/xpgallery/js/yui/css/horizSlider.png")%>'>
							</div>
						</div>
						<asp:Panel ID="pnlVote" Runat="server">
							<P align="left"><%= base.GetText("CURRENTVOTE")%>&nbsp;&nbsp; <SPAN id="lbrating" name="lbrating">
									0 </SPAN>
								<BR>
								<asp:Button id="btnVote" runat="server"></asp:Button></P>
						</asp:Panel>
						<asp:Panel ID="pnlMessage" Runat="server">
							<P>
								<asp:Literal id="litMessage" Runat="server"></asp:Literal></P>
						</asp:Panel>
					</td>
				</tr>
				<tr>
					<td valign="middle" align="center">
						<table border="0" cellpadding="0" cellspacing="0">
							<tr>
								<td width="49%" valign="middle" align="right">
									<a id="lnkPrevious" runat="server" class="xpNextPreviousPhoto">
										<%= base.GetText("PREVIOUS") %>
									</a>&nbsp;<img width="14" height="13" id="imgPrevious" runat="server">
								</td>
								<td width="2%" valign="middle" align="right">
									<img src='<%= ResolveUrl("~/Modules/xpGallery/images/picto_sep.gif.gif")%>' width="1" height="13">
								</td>
								<td width="49%" valign="middle" align="left">
									<img src='<%= ResolveUrl("~/Modules/xpGallery/images/picto_next.gif")%>' width="14" height="13" id="imgNext" runat="server">&nbsp;<a id="lnkNext" runat="server" class="xpNextPreviousPhoto"><%= base.GetText("NEXT") %></a>
								</td>
							</tr>
						</table>
					</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td align="center">
						<div id="loading">
							<img src='<%= ResolveUrl("~/Modules/xpGallery/images/loading.gif")%>' >
						</div>
					</td>
					<td>&nbsp;</td>
				</tr>
			</table>
			<input type="hidden" id="hdgalleryid" runat="server" NAME="hdgallery"> <input type="hidden" id="hdphotoid" runat="server" NAME="hdgalleryid">
			<input type="hidden" id="hdculture" runat="server" NAME="hdgalleryid"> <input type="hidden" id="hdrating" runat="server" NAME="hdrating" value="0">
		</form>
	</body>
</HTML>
