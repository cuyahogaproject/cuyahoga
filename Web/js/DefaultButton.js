
// Sets default buttons.
// Originally created by Janus Kamp Hansen - http://www.kamp-hansen.dk
// Extended by Darrell Norton - http://dotnetjunkies.com/weblog/darrell.norton/ 
// Tidied by Martijn Boland - http://www.cuyahoga-project.org
function fnTrapKD(btnID, event)
{
	var button = document.getElementById(btnID); // only recent browsers
	if (document.all) // IE
	{
		if (event.keyCode == 13)
		{
			event.returnValue = false;
			event.cancel = true;
			button.click();
		}
	}
	else if (document.getElementById)
	{
		if (event.which == 13) 
		{
			event.returnValue = false;
			event.cancel = true;
			button.focus();
			button.click();
		}
	}
}