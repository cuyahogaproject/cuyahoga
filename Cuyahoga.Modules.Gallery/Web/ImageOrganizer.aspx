<%@ Page language="c#" Codebehind="ImageOrganizer.aspx.cs" AutoEventWireup="false" Inherits="Cuyahoga.Modules.Gallery.Web.ImageOrganizer" %>
<%@ Register TagPrefix="uc2" TagName="ImageTable" src="ImageTable.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ImageOrganizer</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="javascript">
		var selected = 0;
		var ar;

		function InitArray()
		{
			ar = new Array(  document.images.length  );
			var i;
			for ( i =0; i < document.images.length; i++ )
			{
				var img = document.images[ i ];
				ar[ i ] = img.name;
			}
			obj = document.getElementById( "imgTable_cell" + selected );
			if (obj != null ) obj.className = "selected";
		}
		
		function SetSelected( x )
		{
			var obj1 = document.getElementById( "imgTable_cell" + selected );
			obj1.className = "normal";
			selected = x;
			var obj2 = document.getElementById( "imgTable_cell" + selected );
			obj2.className = "selected";
		}
		
		function MoveLeft()
		{
			if ( selected == 0 ) return;
			swap( selected - 1, selected );
			SetSelected( selected - 1 )
		}
		
		function MoveRight()
		{
			if ( selected == ar.length - 1 ) return;
			swap( selected + 1, selected );
			SetSelected( selected + 1 )
		}
		
		function swap( x , y )
		{
			var obj1 = document.getElementById( "imgTable_cell" + x );
			var obj2 = document.getElementById( "imgTable_cell" + y );
			var oldstr = obj1.innerHTML;
			obj1.innerHTML = obj2.innerHTML;
			obj2.innerHTML = oldstr;
			var val = ar [ x ];
			ar[ x ] = ar [ y ];
			ar[ y ] = val;
		}
		
		function SubmitChanges()
		{
			var i;
			for ( i = 0; i < ar.length; i++ )
			{
				var obj = document.getElementById( "hd" + i  );
				obj.setAttribute("value", ar[ i ] );
			}
			document.Form1.submit();
		}
		</script>
		<STYLE type="text/css"> 
		.selected { BORDER-RIGHT: red 1px solid; BORDER-TOP: red 1px solid; BORDER-LEFT: red 1px solid; BORDER-BOTTOM: red 1px solid } 
		.normal { BORDER-RIGHT: white 1px solid; BORDER-TOP: white 1px solid; BORDER-LEFT: white 1px solid; BORDER-BOTTOM: white 1px solid } 
		.message { FONT-SIZE: 12px; COLOR: green; FONT-FAMILY: Arial } 
		</STYLE>
	</HEAD>
	<body onload="InitArray();">
		<form id="Form1" method="post" runat="server">
			<P>
				<asp:Label id="lblmessage" runat="server" CssClass="message">&nbsp;</asp:Label>
			</P>
			<P>
				<uc2:imagetable id="imgTable" runat="server" EnableViewState="False"></uc2:imagetable></P>
			<p align="center">
				<button id="btnLeft" onclick="MoveLeft();return false;" type="button">&lt; Move 
					left</button> <button type="button" id="btnRight" onclick="MoveRight(); return false;">
					Move right &gt; </button><button type="button" id="btnSubmit" onclick="SubmitChanges(); return false">
					Save changes</button> <button type="button" id="btnClose" onclick="window.close();">
					Close</button>
			</p>
			<input type="hidden" id="hgalleryid" name="hdgalleryid" runat="server">
			<asp:Panel id="Panel1" runat="server"></asp:Panel>
		</form>
		</FORM>
	</body>
</HTML>
