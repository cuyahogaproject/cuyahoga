//=====================================================================
//  DOM Image Rollover v3 (hover)
//
//  Demo: http://chrispoole.com/scripts/dom_image_rollover_hover
//  Script featured on: Dynamic Drive (http://www.dynamicdrive.com)
//=====================================================================
//  copyright Chris Poole
//  http://chrispoole.com
//  This software is licensed under the MIT License 
//  <http://opensource.org/licenses/mit-license.php>
//=====================================================================

function domRollover() {
	if (navigator.userAgent.match(/Opera (\S+)/)) {
		var operaVersion = parseInt(navigator.userAgent.match(/Opera (\S+)/)[1]);
	}
	if (!document.getElementById||operaVersion <7) return;
	var imgarr=document.getElementsByTagName('img');
	var imgPreload=new Array();
	var imgSrc=new Array();
	var imgClass=new Array();
	for (i=0;i<imgarr.length;i++){
		if (imgarr[i].className.indexOf('domroll')!=-1){
			imgSrc[i]=imgarr[i].getAttribute('src');
			imgClass[i]=imgarr[i].className;
			imgPreload[i]=new Image();
			if (imgClass[i].match(/domroll (\S+)/)) {
				imgPreload[i].src = imgClass[i].match(/domroll (\S+)/)[1]
			}
			imgarr[i].setAttribute('xsrc', imgSrc[i]);
			imgarr[i].onmouseover=function(){
				this.setAttribute('src',this.className.match(/domroll (\S+)/)[1]); this.style.cursor='pointer';
			}
			imgarr[i].onmouseout=function(){
				this.setAttribute('src',this.getAttribute('xsrc'))
			}
		}
	}
}

//
// addLoadEvent()
// Adds event to window.onload without overwriting currently assigned onload functions.
// Function found at Simon Willison's weblog - http://simon.incutio.com/
//
function addLoadEvent(func)
{	
	var oldonload = window.onload;
	if (typeof window.onload != 'function'){
    	window.onload = func;
	} else {
		window.onload = function(){
		oldonload();
		func();
		}
	}
}

addLoadEvent(domRollover);	// run initLightbox onLoad

