/* 
FCKEditor configuration file for Cuyahoga.
Requires jquery.FCKEditor.js, CuyahogaConfig.SiteDataDir and CuyahogaConfig.SupportDir
*/
$(document).ready(function() {
	$('textarea.htmleditor').fck({
		path: CuyahogaConfig.SupportDir + 'fckeditor/',
		height: $('textarea.htmleditor').height()
	});
});
