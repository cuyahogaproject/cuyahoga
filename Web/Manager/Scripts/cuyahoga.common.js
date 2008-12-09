/* Common javascript functionality for Cuyahoga manager, requires jQuery 1.2.x */

// Global cuyahoga manager js configuration properties
var CuyahogaConfig = function()
{
	// Properties
	this.ContentDir = '/Content/';
}

$(document).ready(function() {
	
	// Add AJAX indicator
	$(document.body).ajaxStart(function() {
		$(document.body).append('<div id="loading"><img src="' + CuyahogaConfig.ContentDir + 'images/ajax-loader.gif" alt="loading"/></div>');
		$('#loading').css({position:"fixed", width:"40px", top:"50%", left:"50%"});
	}).ajaxStop(function() {
		$('#loading').remove();
	});
	
	// Handle expand and collapse links
	$('a.expandlink').click(function() {
		$(this).next().slideToggle(300);
		$(this).toggleClass('collapselink');
	});
})

function displayMessages() {
	$("#messagewrapper").fadeIn(800);
	
	$(".close_message").click(function() { 
		$("#messagewrapper").empty(); 
	});
}

function processJsonMessage(messageData) {
	var messageTypes = ['Message','Error','Exception'];
	$.each(messageTypes, function(index, messageType) {
		var messageValue = eval('messageData.' + messageType);
		if (messageValue != "" && messageValue != undefined) {
			$("#messagewrapper").append('<div class="' + messageType.toLowerCase() + 'box"><img src="' + CuyahogaConfig.ContentDir + 'Images/cross.gif" class="close_message" style="float:right;cursor:pointer" alt="Close" />' + messageValue + '</div>');
		}
	});
	displayMessages();
}
