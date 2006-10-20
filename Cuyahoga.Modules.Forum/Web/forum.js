function InsertEmoticon(ctrlid, text)
{
	var txtarea = document.getElementById(ctrlid);
	var oldval = txtarea.isvalid;
	txtarea.isvalid = true;
	text = ' ' + text + ' ';
	if (txtarea.createTextRange && txtarea.caretPos)
	{
		var caretPos	= txtarea.caretPos;
		caretPos.text	= caretPos.text.charAt(caretPos.text.length - 1) == ' ' ? caretPos.text + text + ' ' : caretPos.text + text;
	}
	else
	{
		txtarea.value  += text;
	}
	txtarea.isvalid = true;
	txtarea.focus();
}
