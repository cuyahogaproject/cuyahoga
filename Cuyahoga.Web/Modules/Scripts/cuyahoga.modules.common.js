/* Common javascript functionality for Cuyahoga manager, requires jQuery 1.2.x */

// jQuery extra's

// Delegate function for selector based event handling.
// Thanks to http://www.danwebb.net/2008/2/8/event-delegation-made-easy-in-jquery
jQuery.delegate = function(rules) {
	return function(e) {
		var target = $(e.target);
		for (var selector in rules)
			if (target.is(selector)) return rules[selector].apply(this, $.makeArray(arguments));
	}
}

// Global cuyahoga manager js configuration properties
var CuyahogaConfig = function() {
	// Properties
	this.ContentDir = '/Content/';
	this.ConfirmText = 'Are you sure?';
	this.SupportDir = '/Support/';
	this.SiteDataDir = '/SiteData/1/';
}

$(document).ready(function() {

	// Add AJAX indicator
	$(document.body).ajaxStart(function() {
		$(document.body).append('<div id="loading"><img src="' + CuyahogaConfig.ContentDir + 'images/ajax-loader.gif" alt="loading"/></div>');
		$('#loading').css({ position: "fixed", width: "40px", top: "50%", left: "50%" });
	}).ajaxStop(function() {
		$('#loading').remove();
	});

	$('#contentarea').click($.delegate({
		'.deletelink': function(e) {
			submitAfterConfirm(e.target);
		}
	}))
})

function displayMessages() {
	$("#messagewrapper").slideDown(500);
	
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

function movePartialMessages() {
	// When partial views are rendered, the PartialMessagesFilter might have added some messages to the partial view output. This method 
	// moves these messages to the main messages area.
	if ($('.partialmessagewrapper').children().length > 0 && $('#messagewrapper').length > 0) {
		$('#messagewrapper').empty();
		$('#messagewrapper').append($('.partialmessagewrapper').children());
		displayMessages();
	}
}

function submitAfterConfirm(targetElement) {
	if (confirm(CuyahogaConfig.ConfirmText)) {
		$(targetElement).parents('form').submit();
	}
}
