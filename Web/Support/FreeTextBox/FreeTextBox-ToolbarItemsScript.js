//** FreeTextBox Builtin ToolbarItems Script (2.0.5) ***/
//   by John Dyer
//   http://www.freetextbox.com/
//**********************************************/

function FTB_Bold(ftbName) { 
	FTB_Format(ftbName,'bold'); 
}
function FTB_BulletedList(ftbName) { 
	FTB_Format(ftbName,'insertunorderedlist'); 
}
function FTB_Copy(ftbName) { 
	try {
		FTB_Format(ftbName,'copy'); 
	} catch (e) {
		alert('Your security settings to not allow you to use this command.  Please visit http://www.mozilla.org/editor/midasdemo/securityprefs.html for more information.');
	}

}
function FTB_CreateLink(ftbName) { 
	if (FTB_IsHtmlMode(ftbName)) return;
	editor = FTB_GetIFrame(ftbName);
	editor.focus();
	if (isIE) {
		editor.document.execCommand('createlink','1',null);
	} else {
		var url = prompt('Enter a URL:', 'http://');
		if ((url != null) && (url != ''))  editor.document.execCommand('createlink',false,url);
	}
}
function FTB_Cut(ftbName) { 
	try {
		FTB_Format(ftbName,'cut'); 
	} catch (e) {
		alert('Your security settings to not allow you to use this command.  Please visit http://www.mozilla.org/editor/midasdemo/securityprefs.html for more information.');
	}

}
function FTB_Delete(ftbName) { 
	editor = FTB_GetIFrame(ftbName);
	if (confirm('Do you want to delete all the HTML and text presently in the editor?')) {	
		editor.document.body.innerHTML = '';
		if (isIE) {			
			editor.document.body.innerText = '';
		}
	}
	editor.focus();
}
function FTB_DeleteTableColumn(ftbName) { 
	if (FTB_IsHtmlMode(ftbName)) return;
	var td = this.FTB_GetClosest(ftbName,"td");
	if (!td) {
		return;
	}
	var index = td.cellIndex;
	if (td.parentNode.cells.length == 1) {
		//alert("Don't delete last column");
		return;
	}
	// set the caret first to a position that doesn't disappear
	FTB_SelectNextNode(td);
	var rows = td.parentNode.parentNode.rows;
	for (var i = rows.length; --i >= 0;) {
		var tr = rows[i];
		tr.removeChild(tr.cells[index]);
	}
}
function FTB_DeleteTableRow(ftbName) {
	var tr = FTB_GetClosest(ftbName,"tr");
	if (!tr) {
		return;
	}
	var par = tr.parentNode;
	if (par.rows.length == 1) {
		//alert("Don't delete the last row!");
		return;
	}
	// set the caret first to a position that doesn't
	// disappear.
	FTB_SelectNextNode(tr);
	par.removeChild(tr);
}
function FTB_ieSpellCheck(ftbName) { 
    if (FTB_IsHtmlMode(ftbName)) return;
	if (!isIE) {
		alert('IE Spell is not supported in Mozilla');
		return;
	}
	try {
		var tspell = new ActiveXObject('ieSpell.ieSpellExtension');
		tspell.CheckAllLinkedDocuments(window.document);
	} catch (err){
		if (window.confirm('You need ieSpell to use spell check. Would you like to install it?')){window.open('http://www.iespell.com/download.php');};
	};

}
function FTB_Indent(ftbName) { 
	FTB_Format(ftbName,'indent'); 
}
function FTB_InsertDate(ftbName) { 
	var d = new Date();
	FTB_InsertText(ftbName,d.toLocaleDateString());
}
function FTB_InsertImage(ftbName) { 
	if (FTB_IsHtmlMode(ftbName)) return;
	editor = FTB_GetIFrame(ftbName);
	editor.focus();
    editor.document.execCommand('insertimage',1,'');
}
function FTB_InsertRule(ftbName) { 
	FTB_Format(ftbName,'inserthorizontalrule');
}
function FTB_InsertTableColumnAfter(ftbName) { 
	FTB_InsertColumn(ftbName,true);
}
function FTB_InsertTableColumnBefore(ftbName) { 
	FTB_InsertColumn(ftbName,false);
}
function FTB_InsertTableRowAfter(ftbName) { 
	FTB_InsertTableRow(ftbName,true);
}
function FTB_InsertTableRowBefore(ftbName) { 
	FTB_InsertTableRow(ftbName,false);
}
function FTB_InsertTime(ftbName) { 
	var d = new Date();
	FTB_InsertText(ftbName,d.toLocaleTimeString());
}
function FTB_Italic(ftbName) { 
	FTB_Format(ftbName,'italic'); 
}
function FTB_JustifyRight(ftbName) { 
	FTB_Format(ftbName,'justifyright'); 
}
function FTB_JustifyCenter(ftbName) { 
	FTB_Format(ftbName,'justifycenter'); 
}
function FTB_JustifyFull(ftbName) { 
	FTB_Format(ftbName,'justifyfull'); 
}
function FTB_JustifyLeft(ftbName) { 
	FTB_Format(ftbName,'justifyleft'); 
}
function FTB_NetSpell(ftbName) { 
    if (FTB_IsHtmlMode(ftbName)) return;
	try {
		checkSpellingById(ftbName + '_Editor');
	} catch(e) {
		alert('Netspell libraries not properly linked.');
	}
}
function FTB_NumberedList(ftbName) { 
	FTB_Format(ftbName,'insertorderedlist'); 
}
function FTB_Outdent(ftbName) { 
	FTB_Format(ftbName,'outdent'); 
}
function FTB_Paste(ftbName) { 
	try {
		FTB_Format(ftbName,'paste'); 
	} catch (e) {
		alert('Your security settings to not allow you to use this command.  Please visit http://www.mozilla.org/editor/midasdemo/securityprefs.html for more information.');
	}
}
function FTB_Print(ftbName) { 
	if (isIE) {
		FTB_Format(ftbName,'print'); 
	} else {
		editor = FTB_GetIFrame(ftbName);
		editor.print();
	}
}
function FTB_Redo(ftbName) { 
	FTB_Format(ftbName,'undo'); 
}
function FTB_RemoveFormat(ftbName) { 
	FTB_Format(ftbName,'removeformat'); 
}
function FTB_Save(ftbName) { 
	FTB_CopyHtmlToHidden(ftbName); 
	__doPostBack(ftbName,'Save');
}
function FTB_StrikeThrough(ftbName) { 
	FTB_Format(ftbName,'strikethrough'); 
}
function FTB_SubScript(ftbName) { 
	FTB_Format(ftbName,'subscript'); 
}
function FTB_SuperScript(ftbName) { 
	FTB_Format(ftbName,'superscript'); 
}
function FTB_Underline(ftbName) { 
	FTB_Format(ftbName,'underline'); 
}
function FTB_Undo(ftbName) { 
	FTB_Format(ftbName,'undo'); 
}
function FTB_Unlink(ftbName) { 
	if (FTB_IsHtmlMode(ftbName)) return;
	editor = FTB_GetIFrame(ftbName);
	editor.focus();
    editor.document.execCommand('unlink',false,null);
}
function FTB_SetFontBackColor(ftbName,name,value) {
	editor = FTB_GetIFrame(ftbName);
	
	if (FTB_IsHtmlMode(ftbName)) return;
	editor.focus();
	editor.document.execCommand('backcolor','',value);
}
function FTB_SetFontFace(ftbName,name,value) {
	editor = FTB_GetIFrame(ftbName);
	
	if (FTB_IsHtmlMode(ftbName)) return;
	editor.focus();
	editor.document.execCommand('fontname','',value);
}
function FTB_SetFontForeColor(ftbName,name,value) {
	editor = FTB_GetIFrame(ftbName);
	
	if (FTB_IsHtmlMode(ftbName)) return;
	editor.focus();
	editor.document.execCommand('forecolor','',value);
}
function FTB_SetFontSize(ftbName,name,value) {
	editor = FTB_GetIFrame(ftbName);
	
	if (FTB_IsHtmlMode(ftbName)) return;
	editor.focus();
	editor.document.execCommand('fontsize','',value);
}
function FTB_InsertHtmlMenu(ftbName,name,value) {
	FTB_InsertText(ftbName,value);
}
function FTB_SetParagraph(ftbName,name,value) {
	if (FTB_IsHtmlMode(ftbName)) return;
	editor = FTB_GetIFrame(ftbName);
	if (value == '<body>') {
		editor.document.execCommand('formatBlock','','Normal');
		editor.document.execCommand('removeFormat');
		return;
	}
	editor.document.execCommand('formatBlock','',value);
}
function FTB_SetStyle(ftbName,name,value) { 

}
function FTB_SymbolsMenu(ftbName,name,value) {
	FTB_InsertText(ftbName,value);
}
function FTB_InsertTable(ftbName) {
	editor = FTB_GetIFrame(ftbName);
	editor.focus();
	
	var tableWin = window.open("","tableWin","width=400,height=180");
	tableWin.focus();
	
	tableWin.document.body.innerHTML = "";
	tableWin.document.write("<html>\
<head> \
<style type='text/css'> \
html, body { \
  background: ButtonFace; \
  color: ButtonText; \
  font: 11px Tahoma,Verdana,sans-serif; \
  margin: 0px; \
  padding: 0px; \
} \
body { padding: 5px; } \
table { \
  font: 11px Tahoma,Verdana,sans-serif; \
} \
form p { \
  margin-top: 5px; \
  margin-bottom: 5px; \
} \
.fl { width: 9em; float: left; padding: 2px 5px; text-align: right; } \
.fr { width: 7em; float: left; padding: 2px 5px; text-align: right; } \
fieldset { padding: 0px 10px 5px 5px; } \
select, input, button { font: 11px Tahoma,Verdana,sans-serif; } \
button { width: 70px; } \
.space { padding: 2px; } \
 \
.title { background: #ddf; color: #000; font-weight: bold; font-size: 120%; padding: 3px 10px; margin-bottom: 10px; \
border-bottom: 1px solid black; letter-spacing: 2px; \
} \
form { padding: 0px; margin: 0px; } \
</style> \
<script language='JavaScript'> \
function insertTable() { \
	cols = parseInt(document.getElementById('f_cols').value); \
	rows = parseInt(document.getElementById('f_rows').value); \
	width = document.getElementById('f_width').value; \
	widthUnit = document.getElementById('f_unit').options[document.getElementById('f_unit').selectedIndex].value; \
	align = document.getElementById('f_align').value; \
	cellpadding = document.getElementById('f_padding').value; \
	cellspacing = document.getElementById('f_spacing').value; \
	border = document.getElementById('f_border').value; \
	window.opener.FTB_CreateTable('" + ftbName + "',cols,rows,width,widthUnit,align,cellpadding,cellspacing,border); \
} \
</script> \
</head> \
<body> \
<form action=''> \
 \
<table border='0' style='padding: 0px; margin: 0px'> \
  <tbody> \
  <tr> \
    <td style='width: 4em; text-align: right'>Rows:</td> \
    <td><input type='text' name='rows' id='f_rows' size='5' title='Number of rows' value='2' /></td> \
    <td></td> \
    <td></td> \
    <td></td> \
  </tr> \
  <tr> \
    <td style='width: 4em; text-align: right'>Cols:</td> \
    <td><input type='text' name='cols' id='f_cols' size='5' title='Number of columns' value='4' /></td> \
    <td style='width: 4em; text-align: right'>Width:</td> \
    <td><input type='text' name='width' id='f_width' size='5' title='Width of the table' value='100' /></td> \
    <td><select size='1' name='unit' id='f_unit' title='Width unit'> \
      <option value='%' selected='1'  >Percent</option> \
      <option value='px'              >Pixels</option> \
      <option value='em'              >Em</option> \
    </select></td> \
  </tr> \
  </tbody> \
</table> \
 \
<fieldset style='float: left; margin-left: 5px;'> \
<legend>Layout</legend> \
 \
<div class='space'></div> \
 \
<div class='fl'>Alignment:</div> \
<select size='1' name='align' id='f_align' \
  title='Positioning of this image'> \
  <option value='' selected='1'                >Not set</option> \
  <option value='left'                         >Left</option> \
  <option value='right'                        >Right</option> \
  <option value='texttop'                      >Texttop</option> \
  <option value='absmiddle'                    >Absmiddle</option> \
  <option value='baseline'                     >Baseline</option> \
  <option value='absbottom'                    >Absbottom</option> \
  <option value='bottom'                       >Bottom</option> \
  <option value='middle'                       >Middle</option> \
  <option value='top'                          >Top</option> \
</select> \
 \
<p /> \
 \
<div class='fl'>Border thickness:</div> \
<input type='text' name='border' id='f_border' size='5' value='1' title='Leave empty for no border' /> \
 \
<div class='space'></div> \
 \
</fieldset> \
 \
<fieldset style='float:right; margin-right: 5px;'> \
<legend>Spacing</legend> \
 \
<div class='space'></div> \
 \
<div class='fr'>Cell spacing:</div> \
<input type='text' name='spacing' id='f_spacing' size='5' value='1' \
title='Space between adjacent cells' /> \
 \
<p /> \
 \
<div class='fr'>Cell padding:</div> \
<input type='text' name='padding' id='f_padding' size='5' value='1' \
title='Space between content and border in cell' /> \
 \
<div class='space'></div> \
 \
</fieldset> \
\
<div style='margin-top: 85px; border-top: 1px solid #999; padding: 2px; text-align: right;'> \
<button type='button' name='ok' onclick='insertTable();window.close();'>Insert</button> \
<button type='button' name='cancel' onclick='window.close();'>Cancel</button> \
</div> \
</form> \
</body></html>");
}
